using Newtonsoft.Json;

namespace OE.Models.ActivityStreams
{
	public class CollectionBase<T> : ActivityStreamsObject
		where T : ActivityStreamsBase
	{
		/// <summary>
		/// A non-negative integer specifying the total number of objects contained by the logical 
		/// view of the collection. This number might not reflect the actual number of items 
		/// serialized within the Collection object instance. 
		/// </summary>
		/// <example>
		/// 2
		/// </example>
		[JsonProperty("totalItems")]
		public int? TotalItems { get; set; }

		/// <summary>
		/// In a paged Collection, indicates the page that contains the most recently updated 
		/// member items. 
		/// </summary>
		/// <example>
		/// http://example.org/collection
		/// </example>
		[JsonProperty("current")]
		public T Current { get; set; }

		/// <summary>
		/// In a paged Collection, indicates the furthest preceeding page of items in the 
		/// collection. 
		/// </summary>
		/// <example>
		/// http://example.org/collection?page=0
		/// </example>
		[JsonProperty("first")]
		public T First { get; set; }

		/// <summary>
		/// In a paged Collection, indicates the furthest proceeding page of the collection. 
		/// </summary>
		/// <example>
		/// http://example.org/collection?page=1
		/// </example>
		[JsonProperty("last")]
		public T Last { get; set; }

		protected CollectionBase()
		{
		}
	}
}
