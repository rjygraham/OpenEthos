using Azure.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OE.Api.Configuration;
using OE.Api.Data;
using OE.Api.Identity.Functions;
using OE.Api.Identity.Functions.Configuration;
using OE.Api.Identity.Functions.Services;
using OE.Api.MicrosoftGraph;
using System;
using System.Reflection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace OE.Api.Identity.Functions
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
			JsonConvert.DefaultSettings = () => Constants.Serialization.JsonSerializerSettings;

			builder.Services.AddOptions<IdHintTokenConfiguration>()
			.Configure<IConfiguration>((settings, configuration) =>
			{
				configuration.GetSection("IdTokenHint").Bind(settings);
			});

			builder.Services.AddOptions<CosmosDbConfiguration>()
			.Configure<IConfiguration>((settings, configuration) =>
			{
				configuration.GetSection("CosmosDb").Bind(settings);
			});

			builder.Services.AddOptions<O365GraphConfiguration>()
			.Configure<IConfiguration>((settings, configuration) =>
			{
				configuration.GetSection("O365Graph").Bind(settings);
			});

			builder.Services.AddSingleton(IdTokenHintImplementationFactory);
			builder.Services.AddSingleton(IO365GraphServiceImplementationFactory);
			builder.Services.AddSingleton(CosmosClientImplementationFactory);
			builder.Services.AddSingleton<IInvitationStore, CosmosDbInvitationStore>();
		}

		private IO365GraphService IO365GraphServiceImplementationFactory(IServiceProvider serviceProvider)
		{
			var options = serviceProvider.GetService<IOptions<O365GraphConfiguration>>();
			return new O365GraphService(
				options.Value.TenantId,
				options.Value.ClientId,
				options.Value.ClientSecret,
				options.Value.EmailSenderObjectId
			);
		}

		private CosmosClient CosmosClientImplementationFactory(IServiceProvider serviceProvider)
		{
			var options = serviceProvider.GetService<IOptions<CosmosDbConfiguration>>();
			return new CosmosClient(options.Value.ConnectionString, Constants.CosmosDb.ClientOptions);
		}

		private IIdTokenHintService IdTokenHintImplementationFactory(IServiceProvider serviceProvider)
		{
			var options = serviceProvider.GetService<IOptions<IdHintTokenConfiguration>>();
			return new IdTokenHintService(options.Value);
		}
	}
}
