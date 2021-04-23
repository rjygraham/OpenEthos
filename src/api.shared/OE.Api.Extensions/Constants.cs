using Newtonsoft.Json;
using OE.Models.ActivityStreams;
using System;
using System.Collections.Generic;
using System.Text;

namespace OE.Api.Extensions
{
	public static class Constants
	{
		public static class Headers
		{
			public const string UserObjectId = "x-oe-oid";
			public const string UserDisplayName = "x-oe-name";
			public const string UserPrincipalName = "x-oe-upn";
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
