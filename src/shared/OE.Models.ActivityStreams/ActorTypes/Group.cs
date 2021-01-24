namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Represents a formal or informal collective of Actors. 
	/// </summary>
	public class Group : ActorBase
	{
		public Group()
		{
			Type = typeof(Group).Name;
		}
	}
}
