using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.IdentityModel.Tokens;
using OE.Api.Data.Entities;
using OE.Api.Identity.Functions.Extensions;
using OE.Api.MicrosoftGraph;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace OE.Api.Identity.Functions
{
	public class CosmosDbInvitationFunctions
	{
		private static string issuer = Environment.GetEnvironmentVariable("InvitationTokenIssuer");
		private static string clientId = Environment.GetEnvironmentVariable("InvitationTokenClientId");

		private readonly IO365GraphService graphService;
		private readonly X509SigningCredentials signingCredentials;

		public CosmosDbInvitationFunctions(IO365GraphService graphService, X509SigningCredentials signingCredentials)
		{
			this.graphService = graphService;
			this.signingCredentials = signingCredentials;
		}

		[FunctionName(nameof(ProcessInvitationsAsync))]
		public async Task ProcessInvitationsAsync(
			[CosmosDBTrigger(Data.Constants.CosmosDb.DatabaseId, Data.Constants.CosmosDb.InvitationsContainerId, ConnectionStringSetting = "CosmosDbSqlConnection", LeaseCollectionName = "leases", CreateLeaseCollectionIfNotExists = true)] IReadOnlyList<Microsoft.Azure.Documents.Document> documents,
			ILogger log
		)
		{
			if (documents != null)
			{
				foreach (var document in documents)
				{
					var invitation = document.ToInvitationEntity();

					var jwt = CreateInvitationJwt(invitation);

					var message = CreateInvitationMessage(invitation.Name, invitation.EmailAddress, jwt);

					await graphService.SendEmailAsync(message);
				}
			}
		}

		private string CreateInvitationJwt(InvitationEntity invitation)
		{
			// All parameters send to Azure AD B2C needs to be sent as claims
			var claims = new List<System.Security.Claims.Claim>
			{
				new System.Security.Claims.Claim("email", invitation.EmailAddress, System.Security.Claims.ClaimValueTypes.String, issuer),
				new System.Security.Claims.Claim("name", invitation.Name, System.Security.Claims.ClaimValueTypes.String, issuer)
			};

			// Create the token
			JwtSecurityToken token = new JwtSecurityToken(issuer, clientId, claims, invitation.CreateTimestamp.DateTime, invitation.ExpirationTimestamp.DateTime, signingCredentials);

			// Get the representation of the signed token
			JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();

			return jwtHandler.WriteToken(token);
		}

		private Message CreateInvitationMessage(string name, string emailAddress, string jwt)
		{
			return new Message
			{
				Subject = "OpenEthos Invitation",
				Body = new ItemBody
				{
					ContentType = BodyType.Text,
					Content = $"Hi {name}!\r\nThis is your invitation to join OpenEthos. Open the link below to redeem the invitation and complete sign-up.\r\n\r\nhttps://www.openethos.io/app?invitation={jwt}\r\n\r\nThanks,\r\nOpenEthos Team"
				},
				From = new Recipient()
				{
					EmailAddress = new EmailAddress
					{
						Name = "OpenEthos",
						Address = "hello@openethos.io"
					}
				},
				ToRecipients = new List<Recipient>
				{
					new Recipient()
					{
						EmailAddress = new EmailAddress
						{
							Name = name,
							Address = emailAddress
						}
					}
				}
			};
		}
	}
}
