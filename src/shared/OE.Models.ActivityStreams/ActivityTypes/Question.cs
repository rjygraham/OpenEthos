using System.Collections.Generic;
using Newtonsoft.Json;

namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Represents a question being asked. Question objects are an extension of 
	/// IntransitiveActivity. That is, the Question object is an Activity, but the direct object
	/// is the question itself and therefore it would not contain an object property. 
	/// 
	/// Either of the anyOf and oneOf properties MAY be used to express possible answers, but a 
	/// Question object MUST NOT have both properties.
	/// </summary>
	public class Question : IntransitiveActivity
	{
		/// <summary>
		/// Identifies an exclusive option for a Question. Use of oneOf implies that the Question
		/// can have only a single answer. To indicate that a Question can have multiple answers, 
		/// use anyOf. 
		/// </summary>
		[JsonProperty("oneOf")]
		public HashSet<ActivityStreamsBase> OneOf { get; set; }

		/// <summary>
		/// Identifies an inclusive option for a Question. Use of anyOf implies that the Question
		/// can have multiple answers. To indicate that a Question can have only one answer, use 
		/// oneOf. 
		/// </summary>
		[JsonProperty("anyOf")]
		
		public HashSet<ActivityStreamsBase> AnyOf { get; set; }

		/// <summary>
		/// Indicates that a question has been closed, and answers are no longer accepted. 
		/// </summary>
		[JsonProperty("closed")]
		
		public bool Closed { get; set; }

		public Question()
		{
			Type = typeof(Question).Name;
		}
	}
}
