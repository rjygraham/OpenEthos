using Newtonsoft.Json;
using System.Collections.Generic;

namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// An Activity is a subtype of Object that describes some form of action that may happen,
	/// is currently happening, or has already happened. The Activity type itself serves as an 
	/// abstract base type for all types of activities. It is important to note that the 
	/// Activity type itself does not carry any specific semantics about the kind of action 
	/// being taken. 
	/// </summary>
	public class Activity : IntransitiveActivity
	{
		/// <summary>
		/// Describes an object of any kind.The Object type serves as the base type for most of the 
		/// other kinds of objects defined in the Activity Vocabulary, including other Core types 
		/// such as Activity, IntransitiveActivity, Collection and OrderedCollection.
		/// </summary>
		[JsonProperty("object")]
		public HashSet<ActivityStreamsObject> Object { get; set; }

		public Activity()
		{
			Type = typeof(Activity).Name;
		}
	}
}
