namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Indicates that the actor has left the object. The target and origin typically have no meaning.
	/// </summary>
	public class Leave : Activity
	{
		public Leave()
		{
			Type = typeof(Leave).Name;
		}
	}
}
