using SourceGenSecrets;

namespace OE.Mobile
{
	public static partial class Constants
	{

		public static partial class ApiSettings
		{
			[SourceGenSecret(EnvironmentVariableName = "API_HOST_URL")]
			public static string HostUrl { get; private set; }
		}

		public static partial class AuthenticationSettings
		{
			// Azure AD B2C Coordinates
			[SourceGenSecret(EnvironmentVariableName = "B2C_TENANTID")]
			public static string Tenant { get; private set; }

			[SourceGenSecret(EnvironmentVariableName = "B2C_HOSTNAME")]
			public static string AzureADB2CHostname { get; private set; }

			[SourceGenSecret(EnvironmentVariableName = "B2C_CLIENTID")]
			public static string ClientId { get; private set; }

			[SourceGenSecret(EnvironmentVariableName = "B2C_SU_POLICY")]
			public static string PolicySignUp { get; private set; }

			[SourceGenSecret(EnvironmentVariableName = "B2C_SI_POLICY")]
			public static string PolicySignIn { get; private set; }

			[SourceGenSecret(EnvironmentVariableName = "XAM_MOBILE_APPID")]
			public static string AppId { get; private set; }

			[SourceGenSecret(EnvironmentVariableName = "XAM_ANDROID_SIGHASH")]
			public static string AndroidSigHash { get; private set; } 
			
			public static string[] Scopes { get; private set; } = { "openid", "profile", "offline_access", $"https://{Tenant}/api/profile" };

			public static string AuthorityBase { get; private set; } = $"https://{AzureADB2CHostname}/tfp/{Tenant}/";
			public static string AuthoritySignUp { get; private set; } = $"{AuthorityBase}{PolicySignUp}";
			public static string AuthoritySignIn { get; private set; } = $"{AuthorityBase}{PolicySignIn}";
			public static string AndroidRedirect { get; private set; } = $"msauth://{AppId}/{AndroidSigHash}";
			public static string IosRedirect { get; private set; } = $"msauth.{AppId}://auth";
		}
	}
}
