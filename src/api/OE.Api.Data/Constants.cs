using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using OE.Api.Data.Serialization;
using OE.Models.ActivityStreams;

namespace OE.Api.Data
{
	public static class Constants
	{
		public static class CosmosDb
		{
			public const string DatabaseId = "openethos";
			public const string OutboxContainerId = "outbox";
			public const string InboxContainerId = "inbox";
			public const string InvitationsContainerId = "invitations";
			public const string FollowersContainerId = "followers";
			public const string LeasesContainerId = "leases";

			public static readonly CosmosClientOptions ClientOptions = new CosmosClientOptions
			{
				Serializer = new NewtonsoftJsonCosmosSerializer(Serialization.JsonSerializerSettings)
			};
		}

		public static class Serialization
		{
			public static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore,
				TypeNameHandling = TypeNameHandling.Auto,
				SerializationBinder = new ActivityStreamsSerializationBinder()
			};
		}
	}
}
