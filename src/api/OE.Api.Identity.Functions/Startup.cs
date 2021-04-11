using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OE.Api.Data;
using OE.Api.Identity.Functions;
using OE.Api.MicrosoftGraph;
using System;
using System.Security.Cryptography.X509Certificates;

[assembly: FunctionsStartup(typeof(Startup))]

namespace OE.Api.Identity.Functions
{
	public class Startup : FunctionsStartup
	{
		public override void Configure(IFunctionsHostBuilder builder)
		{
			JsonConvert.DefaultSettings = () => Api.Extensions.Constants.Serialization.JsonSerializerSettings;

			builder.Services.AddSingleton(X509SigningCredentialsImplementationFactory);
			builder.Services.AddSingleton<IO365GraphService>(new O365GraphService(
				Environment.GetEnvironmentVariable("O365GraphTenantId"),
				Environment.GetEnvironmentVariable("O365GraphClientId"),
				Environment.GetEnvironmentVariable("O365GraphClientSecret"),
				Environment.GetEnvironmentVariable("O365GraphEmailSenderObjectId")
			));

			builder.Services.AddSingleton(new CosmosClient(Environment.GetEnvironmentVariable("CosmosDbSqlConnection"), Data.Constants.CosmosDb.ClientOptions));
			builder.Services.AddSingleton<IInvitationStore, CosmosDbInvitationStore>();
		}

		private X509SigningCredentials X509SigningCredentialsImplementationFactory(IServiceProvider serviceProvider)
		{
			var thumbprint = Environment.GetEnvironmentVariable("InvitationTokenSigningCertificateThumbprint");
			var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
			store.Open(OpenFlags.ReadOnly);

			X509Certificate2Collection certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
			return new X509SigningCredentials(certCollection[0]);
		}
	}
}
