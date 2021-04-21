using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OE.Api.Data;
using OE.Api.Outbox.Functions;
using System;

[assembly: FunctionsStartup(typeof(Startup))]

namespace OE.Api.Outbox.Functions
{
	public class Startup : FunctionsStartup
	{
		public override void Configure(IFunctionsHostBuilder builder)
		{
			// Set the default Newtonsoft settings to user the custom ActivityStreamsSerializationBinder 
			JsonConvert.DefaultSettings = () => Api.Extensions.Constants.Serialization.JsonSerializerSettings;

			builder.Services.AddSingleton(new CosmosClient(Environment.GetEnvironmentVariable("CosmosDbSqlConnection"), Constants.CosmosDb.ClientOptions));
			builder.Services.AddSingleton<IOutboxStore, CosmosDbOutboxStore>();
			builder.Services.AddSingleton<IInvitationStore, CosmosDbInvitationStore>();
		}
	}
}
