namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Indicates that the actor dislikes the object. 
	/// </summary>
	public class Dislike : Activity
	{
		public Dislike()
		{
			Type = typeof(Dislike).Name;
		}
	}
}
