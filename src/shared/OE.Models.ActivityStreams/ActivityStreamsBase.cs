using Newtonsoft.Json;
using System.Collections.Generic;

namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Provides common base class for ObjectBase and LinkBase.
	/// </summary>
	public class ActivityStreamsBase 
	{
		/// <summary>
		/// Identifies the Object or Link type.
		/// </summary>
		/// <example>
		/// Link
		/// </example>
		[JsonProperty("$type", Order = -10)]
		public string Type { get; set; }

		/// <summary>
		/// Identifies the context within which the object exists or an activity was performed. 
		/// 
		/// The notion of "context" used is intentionally vague.The intended function is to serve 
		/// as a means of grouping objects and activities that share a common originating context 
		/// or purpose.An example could be all activities relating to a common project or event. 
		/// </summary>
		/// <example>
		/// https://www.w3.org/ns/activitystreams
		/// </example>
		[JsonProperty("context")]
		public ActivityStreamsBase Context { get; set; }

		/// <summary>
		/// Provides the globally unique identifier for an Object or Link. 
		/// </summary>
		/// <example>
		/// http://sally.example.org
		/// </example>
		[JsonProperty("id")]
		public string Id { get; set; }

		/// <summary>
		/// When used on a Link, identifies the MIME media type of the referenced 
		/// resource.
		/// 
		/// When used on an Object, identifies the MIME media type of the value 
		/// of the content property.If not specified, the content property is 
		/// assumed to contain text/html content. 
		/// </summary>
		/// <example>
		/// text/html
		/// </example>
		[JsonProperty("mediaType")]
		public string MediaType { get; set; }

		/// <summary>
		/// A simple, human-readable, plain-text name for the object. HTML markup MUST NOT be 
		/// included. The name MAY be expressed using multiple language-tagged values. 
		/// </summary>
		/// <example>
		/// "An example link"
		/// 
		/// { "en": "A simple note" },
		/// { "es": "Una nota sencilla" },
		/// { "zh-Hans": "一段简单的笔记" }
		/// </example>
		[JsonProperty("name")]
		public IDictionary<string, string> Name { get; set; }

		/// <summary>
		/// A natural language summarization of the object encoded as HTML. Multiple language 
		/// tagged summaries MAY be provided.
		/// </summary>
		[JsonProperty("summary")]
		public IDictionary<string, string> Summary { get; set; }

		protected ActivityStreamsBase()
		{
		}
	}
}
