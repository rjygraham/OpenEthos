namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// A specialized Link that represents an @mention. 
	/// </summary>
	public class Mention : Link
	{
		public Mention()
		{
			Type = typeof(Mention).Name;
		}
	}
}
