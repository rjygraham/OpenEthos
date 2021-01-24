using Newtonsoft.Json;

namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Describes a relationship between two individuals. The subject and object properties are
	/// used to identify the connected individuals. 
	/// 
	/// See 5.2 Representing Relationships Between Entities for additional information.
	/// </summary>
	public class Relationship : ActivityStreamsObject
	{
		/// <summary>
		/// On a Relationship object, the subject property identifies one of the connected 
		/// individuals.For instance, for a Relationship object describing "John is related to 
		/// Sally", subject would refer to John.
		/// </summary>
		[JsonProperty("subject")]
		public ActivityStreamsBase Subject { get; set; }

		[JsonProperty("relationship")]
		public Relationship Relation { get; set; }

		[JsonProperty("object")]
		public ActivityStreamsObject Object { get; set; }

		public Relationship()
		{
			Type = typeof(Relationship).Name;
		}

		public Relationship(string type)
		{
			Type = type;
		}
	}
}
