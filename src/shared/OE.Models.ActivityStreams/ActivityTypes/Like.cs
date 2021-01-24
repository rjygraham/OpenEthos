namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Indicates that the actor likes, recommends or endorses the object. The target and origin 
	/// typically have no defined meaning.
	/// </summary>
	public class Like : Activity
	{
		public Like()
		{
			Type = typeof(Like).Name;
		}
	}
}
