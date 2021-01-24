﻿using Newtonsoft.Json;

namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Used to represent ordered subsets of items from an OrderedCollection.Refer to the Activity
	/// Streams 2.0 Core for a complete description of the OrderedCollectionPage object. 
	/// </summary>
	/// <typeparam name="T">Type of the collection of items.</typeparam>
	/// <typeparam name="P">Type of the next and previous pointers.</typeparam>
	public class OrderedCollectionPage<T, P> : OrderedCollection<T>
		where T: ActivityStreamsBase
		where P: ActivityStreamsBase
	{
		/// <summary>
		/// Identifies the Collection to which a CollectionPage objects items belong. 
		/// </summary>
		/// <example>
		///	http://example.org/collection
		/// </example>
		[JsonProperty("partOf")]
		public string PartOf { get; set; }

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

		/// <summary>
		/// A non-negative integer value identifying the relative position within the logical 
		/// view of a strictly ordered collection. 
		/// </summary>
		/// <example>
		/// 0
		/// </example>
		[JsonProperty("startIndex")]
		public int? StartIndex { get; set; }
		public OrderedCollectionPage()
		{
			Type = typeof(OrderedCollectionPage<T, P>).Name;
		}
	}
}
