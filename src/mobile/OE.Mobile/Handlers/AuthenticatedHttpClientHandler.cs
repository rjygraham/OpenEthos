using OE.Mobile.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace OE.Mobile.Handlers
{
	internal class AuthenticatedHttpClientHandler : HttpClientHandler
	{
		private readonly IAuthenticationService authenticationService;

		public AuthenticatedHttpClientHandler(IAuthenticationService authenticationService)
		{
			this.authenticationService = authenticationService;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var auth = request.Headers.Authorization;
			if (auth != null)
			{
				var token = await authenticationService.GetAccessTokenAsync().ConfigureAwait(false);
				request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, token);
			}

			return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
		}
	}
}
