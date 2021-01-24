namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Indicates that the actor is removing the object. If specified, the origin indicates the 
	/// context from which the object is being removed. 
	/// </summary>
	public class Remove : Activity
	{
		public Remove()
		{
			Type = typeof(Remove).Name;
		}
	}
}
