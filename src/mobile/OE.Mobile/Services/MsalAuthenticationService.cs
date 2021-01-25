using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OE.Mobile.Services
{
	public class MsalAuthenticationService : IAuthenticationService
	{
		private readonly Lazy<IPublicClientApplication> publicClientApplication;

		public string AccessToken { get; private set; }

		public MsalAuthenticationService(IParentWindowLocatorService parentWindowLocatorService)
		{
			publicClientApplication = new Lazy<IPublicClientApplication>(() =>
			{
				var builder = PublicClientApplicationBuilder.Create(Constants.AuthenticationSettings.ClientId)
				   .WithB2CAuthority(Constants.AuthenticationSettings.AuthoritySignInSignUp);

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
				AccessToken = await AcquireTokenSilentAsync();
			}
			catch (MsalUiRequiredException)
			{
				// acquire token interactive
				AccessToken = await SignInInteractivelyAsync(string.Empty);
			}
			catch (Exception ex)
			{
				// swallow
			}

			if (!string.IsNullOrEmpty(AccessToken))
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

				AccessToken = string.Empty;

				return true;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				return false;
			}
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
			AuthenticationResult authResult = await publicClientApplication.Value.AcquireTokenSilent(Constants.AuthenticationSettings.Scopes, GetAccountByPolicy(accounts, Constants.AuthenticationSettings.PolicySignUpSignIn))
			   .WithB2CAuthority(Constants.AuthenticationSettings.AuthoritySignInSignUp)
			   .ExecuteAsync();

			return authResult.AccessToken;
		}

		private async Task<string> SignInInteractivelyAsync(string domain)
		{
			var accounts = await publicClientApplication.Value.GetAccountsAsync();

			try
			{
				var authResult = await publicClientApplication.Value.AcquireTokenInteractive(Constants.AuthenticationSettings.Scopes)
					.WithAccount(GetAccountByPolicy(accounts, Constants.AuthenticationSettings.PolicySignUpSignIn))
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
	}
}
