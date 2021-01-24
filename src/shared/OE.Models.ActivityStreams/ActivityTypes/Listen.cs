namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Indicates that the actor has listened to the object. 
	/// </summary>
	public class Listen : Activity
	{
		public Listen()
		{
			Type = typeof(Listen).Name;
		}
	}
}
