using Newtonsoft.Json;
using OE.Models.ActivityStreams;
using System;
using System.Collections.Generic;
using System.Text;

namespace OE.Api.Data.Entities
{
	public class OutboxEntity
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("userId")]
		public string UserId { get; set; }

		[JsonProperty("ts")]
		public long Timestamp { get; set; }

		[JsonProperty("data")]
		public ActivityStreamsObject Data { get; set; }
	}
}
