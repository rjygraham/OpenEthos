using Newtonsoft.Json;

namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Represents an individual person.
	/// </summary>
	public class Person : ActorBase
	{
		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("username")]
		public string Username { get; set; }

		public Person()
		{
			Type = typeof(Person).Name;
		}
	}
}
