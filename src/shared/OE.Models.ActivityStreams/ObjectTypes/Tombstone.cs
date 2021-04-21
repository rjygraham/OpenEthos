using System;
using Newtonsoft.Json;

namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// A Tombstone represents a content object that has been deleted.It can be used in Collection
	/// s to signify that there used to be an object at this position, but it has been deleted.
	/// </summary>
	public class Tombstone : ActivityStreamsObject
	{
		/// <summary>
		/// On a Tombstone object, the formerType property identifies the type of the object that
		/// was deleted.
		/// </summary>
		[JsonProperty("formerType")]
		public string FormerType { get; set; }

		/// <summary>
		/// On a Tombstone object, the deleted property is a timestamp for when the object was
		/// deleted.
		/// </summary>
		[JsonProperty("deleted")]
		public DateTimeOffset? Deleted { get; set; }

		public Tombstone()
		{
			Type = typeof(Tombstone).Name;
		}
	}
}
