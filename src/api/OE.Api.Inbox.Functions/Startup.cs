using Azure.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OE.Api.Inbox.Functions;
using System;
using System.Reflection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace OE.Api.Inbox.Functions
{
	class Startup : FunctionsStartup
	{
		public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
		{
			var builtConfig = builder.ConfigurationBuilder.Build();
			var keyVaultEndpoint = builtConfig["AzureKeyVaultEndpoint"];

			if (!string.IsNullOrEmpty(keyVaultEndpoint))
			{
				// Running in Azure.
				builder.ConfigurationBuilder
					.SetBasePath(Environment.CurrentDirectory)
					.AddAzureKeyVault(new Uri(keyVaultEndpoint), new DefaultAzureCredential())
					.AddEnvironmentVariables()
					.Build();
			}
			else
			{
				// Running locally.
				builder.ConfigurationBuilder
				   .SetBasePath(Environment.CurrentDirectory)
				   .AddJsonFile("local.settings.json", true)
				   .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
				   .AddEnvironmentVariables()
				   .Build();
			}
		}

		public override void Configure(IFunctionsHostBuilder builder)
		{
			// Set the default Newtonsoft settings to user the custom ActivityStreamsSerializationBinder 
			JsonConvert.DefaultSettings = () => Api.Extensions.Constants.Serialization.JsonSerializerSettings;
		}
	}
}
