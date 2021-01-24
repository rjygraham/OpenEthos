using Newtonsoft.Json;

namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// A Profile is a content object that describes another Object, typically used to describe
	/// Actor Type objects.The describes property is used to reference the object being described
	/// by the profile.
	/// </summary>
	public class Profile : ActivityStreamsObject
	{
		/// <summary>
		/// On a Profile object, the describes property identifies the object described by the 
		/// Profile. 
		/// </summary>
		[JsonProperty("describes")]
		public ActivityStreamsObject Describes { get; set; }

		public Profile()
		{
			Type = typeof(Profile).Name;
		}
	}
}
