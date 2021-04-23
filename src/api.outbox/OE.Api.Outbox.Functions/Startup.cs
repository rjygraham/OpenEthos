using Azure.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OE.Api.Configuration;
using OE.Api.Data;
using OE.Api.Outbox.Functions;
using System;
using System.Reflection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace OE.Api.Outbox.Functions
{
	public class Startup : FunctionsStartup
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

			builder.Services.AddOptions<CosmosDbConfiguration>()
			.Configure<IConfiguration>((settings, configuration) =>
			{
				configuration.GetSection("CosmosDb").Bind(settings);
			});

			builder.Services.AddSingleton(CosmosClientImplementationFactory);
			builder.Services.AddSingleton<IOutboxStore, CosmosDbOutboxStore>();
			builder.Services.AddSingleton<IInvitationStore, CosmosDbInvitationStore>();
		}

		private CosmosClient CosmosClientImplementationFactory(IServiceProvider serviceProvider)
		{
			var options = serviceProvider.GetService<IOptions<CosmosDbConfiguration>>();
			return new CosmosClient(options.Value.ConnectionString, Constants.CosmosDb.ClientOptions);
		}
	}
}
