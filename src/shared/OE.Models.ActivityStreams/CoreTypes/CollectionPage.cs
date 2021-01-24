using Newtonsoft.Json;

namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Used to represent distinct subsets of items from a Collection. Refer to the Activity 
	/// Streams 2.0 Core for a complete description of the CollectionPage object.
	/// </summary>
	/// <typeparam name="T">Type of the collection of items.</typeparam>
	/// <typeparam name="P">Type of the next and previous pointers.</typeparam>
	public class CollectionPage<T, P> : Collection<T>
		where T : ActivityStreamsBase
		where P : ActivityStreamsBase
	{
		/// <summary>
		/// Identifies the Collection to which a CollectionPage objects items belong. 
		/// </summary>
		/// <example>
		/// http://example.org/collection
		/// </example>
		[JsonProperty("partOf")]
		public ActivityStreamsBase PartOf { get; set; }

		/// <summary>
		/// In a paged Collection, indicates the next page of items. 
		/// </summary>
		/// <example>
		/// http://example.org/collection?page=2
		/// </example>
		[JsonProperty("next")]
		public P Next { get; set; }

		/// <summary>
		/// In a paged Collection, identifies the previous page of items. 
		/// </summary>
		/// <example>
		/// http://example.org/collection?page=1
		/// </example>
		[JsonProperty("prev")]
		
		public P Prev { get; set; }

		public CollectionPage()
		{
			Type = typeof(CollectionPage<T, P>).Name;
		}
	}
}
