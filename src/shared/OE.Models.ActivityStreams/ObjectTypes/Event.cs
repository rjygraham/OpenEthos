namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Represents any kind of event.
	/// </summary>
	public class Event : ActivityStreamsObject
	{
		public Event()
		{
			Type = typeof(Event).Name;
		}
	}
}
