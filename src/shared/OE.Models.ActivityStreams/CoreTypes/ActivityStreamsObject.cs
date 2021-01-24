using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Describes an object of any kind.The Object type serves as the base type for most of the 
	/// other kinds of objects defined in the Activity Vocabulary, including other Core types 
	/// such as Activity, IntransitiveActivity, Collection and OrderedCollection.
	/// </summary>
	public class ActivityStreamsObject : ActivityStreamsBase
	{
#pragma warning disable S4004 // Collection properties should be readonly. Rationale: Any number of these properties may need to be used. It'd be wasteful to eargerly initialized or create Setter methods for all properties.

		/// <summary>
		/// Identifies a resource attached or related to an object that potentially requires 
		/// special handling. The intent is to provide a model that is at least semantically 
		/// similar to attachments in email. 
		/// </summary>
		[JsonProperty("attachment")]

		public HashSet<ActivityStreamsBase> Attachment { get; set; }

		/// <summary>
		/// Identifies one or more entities to which this object is attributed. The attributed 
		/// entities might not be Actors. For instance, an object might be attributed to the 
		/// completion of another activity. 
		/// </summary>
		[JsonProperty("attributedTo")]
		public HashSet<ActivityStreamsBase> AttributedTo { get; set; }

		/// <summary>
		/// Identifies one or more entities that represent the total population of entities for 
		/// which the object can considered to be relevant. 
		/// </summary>
		[JsonProperty("audience")]
		public HashSet<ActorBase> Audience { get; set; }

		/// <summary>
		/// The content or textual representation of the Object encoded as a JSON string. By 
		/// default, the value of content is HTML. The mediaType property can be used in the 
		/// object to indicate a different content type. 
		/// 
		/// The content MAY be expressed using multiple language-tagged values.
		/// </summary>
		[JsonProperty("content")]
		public IDictionary<string, string> Content { get; set; }

		/// <summary>
		/// The date and time describing the actual or expected ending time of the object. When used 
		/// with an Activity object, for instance, the endTime property specifies the moment the 
		/// activity concluded or is expected to conclude. 
		/// </summary>
		[JsonProperty("endTime")]
		public DateTime? EndTime { get; set; }

		/// <summary>
		/// Identifies the entity (e.g. an application) that generated the object. 
		/// </summary>
		[JsonProperty("generator")]
		public ActorBase Generator { get; set; }

		/// <summary>
		/// Indicates an entity that describes an icon for this object. The image should have an
		/// aspect ratio of one (horizontal) to one (vertical) and should be suitable for 
		/// presentation at a small size.
		/// </summary>
		[JsonProperty("icon")]
		public Image Icon { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("image")]
		public Image Image { get; set; }

		/// <summary>
		/// Indicates one or more entities for which this object is considered a response. 
		/// </summary>
		[JsonProperty("inReplyTo")]
		public ActivityStreamsBase InReplyTo { get; set; }

		/// <summary>
		/// Indicates one or more physical or logical locations associated with the object. 
		/// </summary>
		[JsonProperty("location")]
		public HashSet<ActivityStreamsBase> Location { get; set; }

		/// <summary>
		/// Identifies an entity that provides a preview of this object. 
		/// </summary>
		[JsonProperty("preview")]
		public ActivityStreamsBase Preview { get; set; }

		/// <summary>
		/// The date and time at which the object was published
		/// </summary>
		[JsonProperty("published")]
		public DateTime? Published { get; set; }

		/// <summary>
		/// Identifies a Collection containing objects considered to be responses to this object. 
		/// </summary>
		[JsonProperty("replies")]
		public Collection<ActivityStreamsObject> Replies { get; set; }

		/// <summary>
		/// The date and time describing the actual or expected starting time of the object. When 
		/// used with an Activity object, for instance, the startTime property specifies the 
		/// moment the activity began or is scheduled to begin.
		/// </summary>
		[JsonProperty("startTime")]
		public DateTime? StartTime { get; set; }

		/// <summary>
		/// One or more "tags" that have been associated with an objects. A tag can be any kind of 
		/// Object. The key difference between attachment and tag is that the former implies 
		/// association by inclusion, while the latter implies associated by reference. 
		/// </summary>
		[JsonProperty("tag")]
		public HashSet<ActivityStreamsBase> Tag { get; set; }

		/// <summary>
		/// The date and time at which the object was updated.
		/// </summary>
		[JsonProperty("updated")]
		public DateTime? Updated { get; set; }

		/// <summary>
		/// Identifies one or more links to representations of the object 
		/// </summary>
		[JsonProperty("url")]
		public HashSet<Link> Url { get; set; }

		/// <summary>
		/// Identifies an entity considered to be part of the public primary audience of an Object 
		/// </summary>
		[JsonProperty("to")]
		public HashSet<ActorBase> To { get; set; }

		/// <summary>
		/// Identifies an Object that is part of the private primary audience of this Object. 
		/// </summary>
		[JsonProperty("bto")]
		public HashSet<ActorBase> Bto { get; set; }

		/// <summary>
		/// Identifies an Object that is part of the public secondary audience of this Object.
		/// </summary>
		[JsonProperty("cc")]
		public HashSet<ActorBase> Cc { get; set; }

		/// <summary>
		/// Identifies one or more Objects that are part of the private secondary audience of this 
		/// Object. 
		/// </summary>
		[JsonProperty("bcc")]
		public HashSet<ActorBase> Bcc { get; set; }

		/// <summary>
		/// When the object describes a time-bound resource, such as an audio or video, a meeting, etc,
		/// the duration property indicates the object's approximate duration. The value MUST be 
		/// expressed as an xsd:duration as defined by [ xmlschema11-2], section 3.3.6 (e.g. a 
		/// period of 5 seconds is represented as "PT5S"). 
		/// </summary>
		[JsonProperty("duration")]
		public TimeSpan? Duration { get; set; }

#pragma warning restore S4004 // Collection properties should be readonly
	}
}
