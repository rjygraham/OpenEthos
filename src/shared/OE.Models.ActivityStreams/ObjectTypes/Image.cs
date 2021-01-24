using Newtonsoft.Json;

namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// An image document of any kind 
	/// </summary>
	public class Image : Document
	{
		[JsonProperty("height")]
		public int? Height { get; set; }

		[JsonProperty("width")]
		public int? Width { get; set; }

		public Image()
		{
			Type = typeof(Image).Name;
		}
	}
}
