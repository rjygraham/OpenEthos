namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Indicates that the actor has read the object. 
	/// </summary>
	public class Read : Activity
	{
		public Read()
		{
			Type = typeof(Read).Name;
		}
	}
}
