namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Indicates that the actor has created the object.
	/// </summary>
	public class Create : Activity
	{
		public Create()
		{
			Type = typeof(Create).Name;
		}
	}
}
