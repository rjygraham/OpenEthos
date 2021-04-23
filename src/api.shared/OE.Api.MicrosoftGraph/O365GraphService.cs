using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System.Threading.Tasks;

namespace OE.Api.MicrosoftGraph
{
	public class O365GraphService : IO365GraphService
	{
		private readonly GraphServiceClient client;
		private readonly string emailSenderObjectId;

		public O365GraphService(string tenantId, string clientId, string clientSecret, string emailSenderObjectId)
		{
			var confidentialClientApplication = ConfidentialClientApplicationBuilder
					.Create(clientId)
					.WithTenantId(tenantId)
					.WithClientSecret(clientSecret)
					.Build();

			var authProvider = new ClientCredentialProvider(confidentialClientApplication);

			client = new GraphServiceClient(authProvider);

			this.emailSenderObjectId = emailSenderObjectId;
		}

		public async Task SendEmailAsync(Message message)
		{
			await client.Users[emailSenderObjectId]
				.SendMail(message, false)
				.Request()
				.WithMaxRetry(2)
				.PostAsync();
		}
	}
}
