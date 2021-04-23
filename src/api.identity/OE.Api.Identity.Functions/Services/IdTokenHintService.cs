using Microsoft.IdentityModel.Tokens;
using OE.Api.Identity.Functions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;

namespace OE.Api.Identity.Functions.Services
{
	public class IdTokenHintService : IIdTokenHintService
	{
		private readonly string issuer;
		private readonly string clientId;
		private readonly X509SigningCredentials signingCredentials;
		private readonly JwtSecurityTokenHandler jwtHandler;

		public IdTokenHintService(IdHintTokenConfiguration config)
		{
			issuer = config.Issuer;
			clientId = config.ClientId;
			signingCredentials = GetX509SigningCredentials(config.SigningCertificateThumbprint);
			jwtHandler = new JwtSecurityTokenHandler();
		}

		public string GetIdTokenHint(string emailAddress, string ancestorId, DateTime notBefore, DateTime expires)
		{
			// All parameters send to Azure AD B2C needs to be sent as claims
			var claims = new List<System.Security.Claims.Claim>
			{
				new System.Security.Claims.Claim("invitation_email", emailAddress, System.Security.Claims.ClaimValueTypes.String, issuer),
				new System.Security.Claims.Claim("extension_ancestorId", ancestorId, System.Security.Claims.ClaimValueTypes.String, issuer),
				new System.Security.Claims.Claim("extension_sponsorId", ancestorId, System.Security.Claims.ClaimValueTypes.String, issuer)
			};

			// Create the token
			var token = new JwtSecurityToken(issuer, clientId, claims, notBefore,  expires, signingCredentials);
			return jwtHandler.WriteToken(token);
		}

		private X509SigningCredentials GetX509SigningCredentials(string thumbprint)
		{
			var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
			store.Open(OpenFlags.ReadOnly);

			X509Certificate2Collection certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
			return new X509SigningCredentials(certCollection[0]);
		}
	}
}
