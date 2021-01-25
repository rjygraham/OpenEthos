using System;
using System.Collections.Generic;
using System.Text;

namespace OE.Mobile
{
	public static class Constants
	{
		public static class AuthenticationSettings
		{
			// Azure AD B2C Coordinates
			public const string Tenant = "twitchoedev.onmicrosoft.com";
			public const string AzureADB2CHostname = "twitchoedev.b2clogin.com";
			public const string ClientId = "d709ef3a-4134-46a0-9d1d-d7c335e5b728";
			public const string PolicySignUpSignIn = "B2C_1_signup_signin";

			public static string[] Scopes = { "openid", "profile", "offline_access", "https://twitchoedev.onmicrosoft.com/api/profile" };

			public static string AuthorityBase = $"https://{AzureADB2CHostname}/tfp/{Tenant}/";
			public static string AuthoritySignInSignUp = $"{AuthorityBase}{PolicySignUpSignIn}";

			public static string AppId = "com.companyname.openethos";

			public static string AndroidRedirect = $"msauth://{AppId}/T60n5E0JVPDmhnQda%2B%2F%2BG0eJV6M%3D";
			public static string IosRedirect = $"msauth.{AppId}://auth";
		}
	}
}
