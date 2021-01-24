using Newtonsoft.Json;

namespace OE.Models.ActivityStreams
{
	public static class ActivityStreamsJsonSerializerSettings
	{
		public static readonly JsonSerializerSettings Instance = new JsonSerializerSettings 
		{
			TypeNameHandling = TypeNameHandling.Auto,
			NullValueHandling = NullValueHandling.Ignore,
			SerializationBinder = new ActivityStreamsSerializationBinder()
		};
	}
}
