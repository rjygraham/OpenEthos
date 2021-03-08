using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OE.Mobile.Services
{
	public class MsalAuthenticationService : IAuthenticationService
	{
		private readonly Lazy<IPublicClientApplication> publicClientApplication;
		private string accessToken;

		public MsalAuthenticationService(IParentWindowLocatorService parentWindowLocatorService)
		{
			publicClientApplication = new Lazy<IPublicClientApplication>(() =>
			{
				var builder = PublicClientApplicationBuilder.Create(Constants.AuthenticationSettings.ClientId)
				   .WithB2CAuthority(Constants.AuthenticationSettings.AuthoritySignIn);

				switch (Device.RuntimePlatform)
				{
					case Device.Android:
						builder
							.WithRedirectUri(Constants.AuthenticationSettings.AndroidRedirect)
							.WithParentActivityOrWindow(() => parentWindowLocatorService.GetCurrentParentWindow());
						break;
					case Device.iOS:
						builder
							.WithRedirectUri(Constants.AuthenticationSettings.IosRedirect)
							.WithIosKeychainSecurityGroup(Constants.AuthenticationSettings.AppId);
						break;
					default:
						break;
				}
				return builder.Build();
			});
		}

		public async Task<bool> LoginAsync()
		{
			try
			{
				// acquire token silent
				accessToken = await AcquireTokenSilentAsync();
			}
			catch (MsalUiRequiredException)
			{
				// acquire token interactive
				accessToken = await SignInInteractivelyAsync(string.Empty);
			}
			catch (Exception ex)
			{
				// swallow
			}

			if (!string.IsNullOrEmpty(accessToken))
			{
				//appStateService.IsLoggedIn = context.IsLoggedIn;
				//return context.IsLoggedIn;

				return true;
			}

			return false;
		}

		public async Task<bool> LogoutAsync()
		{
			try
			{
				var accounts = await publicClientApplication.Value.GetAccountsAsync();

				// Go through all accounts and remove them.
				while (accounts.Any())
				{
					await publicClientApplication.Value.RemoveAsync(accounts.FirstOrDefault());
					accounts = await publicClientApplication.Value.GetAccountsAsync();
				}

				accessToken = string.Empty;

				return true;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				return false;
			}
		}

		public async Task<string> GetAccessTokenAsync()
		{
			var token = await AcquireTokenSilentAsync().ConfigureAwait(false);
			return token;
		}

		private IAccount GetAccountByPolicy(IEnumerable<IAccount> accounts, string policy)
		{
			foreach (var account in accounts)
			{
				string userIdentifier = account.HomeAccountId.ObjectId.Split('.')[0];
				if (userIdentifier.EndsWith(policy.ToLower())) return account;
			}

			return null;
		}

		private async Task<string> AcquireTokenSilentAsync()
		{
			IEnumerable<IAccount> accounts = await publicClientApplication.Value.GetAccountsAsync();
			AuthenticationResult authResult = await publicClientApplication.Value.AcquireTokenSilent(Constants.AuthenticationSettings.Scopes, GetAccountByPolicy(accounts, Constants.AuthenticationSettings.PolicySignIn))
			   .WithB2CAuthority(Constants.AuthenticationSettings.AuthoritySignIn)
			   .ExecuteAsync();

			return authResult.AccessToken;
		}

		private async Task<string> SignInInteractivelyAsync(string domain)
		{
			var accounts = await publicClientApplication.Value.GetAccountsAsync();

			try
			{
				var authResult = await publicClientApplication.Value.AcquireTokenInteractive(Constants.AuthenticationSettings.Scopes)
					.WithUseEmbeddedWebView(true)
					.WithAccount(GetAccountByPolicy(accounts, Constants.AuthenticationSettings.PolicySignIn))
					//.WithExtraQueryParameters($"domain_hint={domain}")
					.ExecuteAsync();

				// var context = UpdateUserInfo(authResult);

				return authResult.AccessToken;
			}
			catch (MsalClientException mcex)
			{
				if (mcex.ErrorCode.Equals("authentication_canceled", StringComparison.OrdinalIgnoreCase))
				{
					// user cancelled authentication.
				}
			}
			catch (MsalServiceException msex)
			{
				if (msex.ErrorCode.Equals("access_denied", StringComparison.OrdinalIgnoreCase))
				{
					// user cancelled self-assertion.
				}
			}

			return null;
		}

		public async Task<bool> SignUpAsync(string idTokenHint, string domainHint)
		{
			var accounts = await publicClientApplication.Value.GetAccountsAsync();

			try
			{
				var authResult = await publicClientApplication.Value.AcquireTokenInteractive(Constants.AuthenticationSettings.Scopes)
					.WithUseEmbeddedWebView(true)
					.WithB2CAuthority(Constants.AuthenticationSettings.AuthoritySignUp)
					.WithAccount(GetAccountByPolicy(accounts, Constants.AuthenticationSettings.PolicySignIn))
					.WithExtraQueryParameters(new Dictionary<string, string>
					{
						{ "domain_hint", domainHint },
						{ "id_token_hint", idTokenHint }
					})
					.ExecuteAsync();

				accessToken = authResult.AccessToken;
				return true;
			}
			catch (MsalClientException mcex)
			{
				if (mcex.ErrorCode.Equals("authentication_canceled", StringComparison.OrdinalIgnoreCase))
				{
					// user cancelled authentication.
				}
			}
			catch (MsalServiceException msex)
			{
				if (msex.ErrorCode.Equals("access_denied", StringComparison.OrdinalIgnoreCase))
				{
					// user cancelled self-assertion.
				}
			}

			return false;
		}
	}
}
