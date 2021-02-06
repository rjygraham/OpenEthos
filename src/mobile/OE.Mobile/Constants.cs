using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OE.Mobile
{
	public static class Constants
	{
		public static class AuthenticationSettings
		{
			// Azure AD B2C Coordinates
			public const string Tenant = "$B2C_TENANTID$";
			public const string AzureADB2CHostname = "$B2C_HOSTNAME$";
			public const string ClientId = "$B2C_CLIENTID$";
			public const string PolicySignUpSignIn = "$B2C_SUSI_POLICY$";

			public static string[] Scopes = { "openid", "profile", "offline_access", $"https://{Tenant}/api/profile" };

			public static string AuthorityBase = $"https://{AzureADB2CHostname}/tfp/{Tenant}/";
			public static string AuthoritySignInSignUp = $"{AuthorityBase}{PolicySignUpSignIn}";

			public static string AppId = "$XAM_MOBILE_APPID$";

			public static string AndroidRedirect = $"msauth://{AppId}/$XAM_ANDROID_SIGHASH$";
			public static string IosRedirect = $"msauth.{AppId}://auth";
		}
	}
}