using Newtonsoft.Json;

namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Represents a logical or physical location. See 5.3 Representing Places for additional 
	/// information. 
	/// </summary>
	public class Place : ActivityStreamsObject
	{
		/// <summary>
		/// Indicates the accuracy of position coordinates on a Place objects. Expressed in 
		/// properties of percentage. e.g. "94.0" means "94.0% accurate". 
		/// </summary>
		[JsonProperty("accuracy")]
		public double? Accuracy { get; set; }

		/// <summary>
		/// Indicates the altitude of a place. The measurement units is indicated using the units
		/// property. If units is not specified, the default is assumed to be "m" indicating meters. 
		/// </summary>
		[JsonProperty("altitude")]
		public double? Altitude { get; set; }

		/// <summary>
		/// The latitude of a place 
		/// </summary>
		[JsonProperty("latitude")]
		public double? Latitude { get; set; }

		/// <summary>
		/// The longitude of a place 
		/// </summary>
		[JsonProperty("longitude")]
		public double? Longitude { get; set; }

		/// <summary>
		/// The radius from the given latitude and longitude for a Place. The units is expressed by the 
		/// units property. If units is not specified, the default is assumed to be "m" indicating 
		/// "meters". 
		/// </summary>
		[JsonProperty("radius")]
		public double? Radius { get; set; }

		/// <summary>
		/// Specifies the measurement units for the radius and altitude properties on a Place 
		/// object. If not specified, the default is assumed to be "m" for "meters".
		/// </summary>
		[JsonProperty("units")]
		public string Units { get; set; }

		public Place()
		{
			Type = typeof(Place).Name;
		}
	}
}
