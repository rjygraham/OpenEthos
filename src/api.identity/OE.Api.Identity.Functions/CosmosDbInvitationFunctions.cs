using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using OE.Api.Identity.Functions.Extensions;
using OE.Api.Identity.Functions.Services;
using OE.Api.MicrosoftGraph;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OE.Api.Identity.Functions
{
	public class CosmosDbInvitationFunctions
	{
		
		private readonly IO365GraphService graphService;
		private readonly IIdTokenHintService idTokenHintService;

		public CosmosDbInvitationFunctions(IO365GraphService graphService, IIdTokenHintService idTokenHintService)
		{
			this.graphService = graphService;
			this.idTokenHintService = idTokenHintService;
		}

		[FunctionName(nameof(ProcessInvitationsAsync))]
		public async Task ProcessInvitationsAsync(
			[CosmosDBTrigger(Data.Constants.CosmosDb.DatabaseId, Data.Constants.CosmosDb.InvitationsContainerId, ConnectionStringSetting = "CosmosDb:ConnectionString", LeaseCollectionName = "leases")] IReadOnlyList<Microsoft.Azure.Documents.Document> documents,
			ILogger log
		)
		{
			if (documents != null)
			{
				foreach (var document in documents)
				{
					var invitation = document.ToInvitationEntity();

					var jwt = idTokenHintService.GetIdTokenHint(invitation.EmailAddress, invitation.AncestorId, invitation.CreateTimestamp.UtcDateTime, invitation.ExpirationTimestamp.UtcDateTime);

					var message = CreateInvitationMessage(invitation.InviterName, invitation.InviteeName, invitation.EmailAddress, jwt);

					await graphService.SendEmailAsync(message);
				}
			}
		}

		private Message CreateInvitationMessage(string inviterName, string inviteeName, string emailAddress, string jwt)
		{
			return new Message
			{
				Subject = "OpenEthos Invitation",
				Body = new ItemBody
				{
					ContentType = BodyType.Html,
					Content = $@"<p>Hi {inviteeName}!</p>
					<p>Your friend {inviterName} has invited you to join OpenEthos. Use the link below to complete sign-up process using your Apple, Google, or Microsoft account associated with this email address.</p>
					<p><a href=""https://www.openethos.io/app?invitation={jwt}"">Redeem Invitation</a></p>
					<p>Thanks,
					<br />OpenEthos Team</p>"
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
							Name = inviteeName,
							Address = emailAddress
						}
					}
				}
			};
		}
	}
}
