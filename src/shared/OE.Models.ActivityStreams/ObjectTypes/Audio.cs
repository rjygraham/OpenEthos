namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Represents an audio document of any kind. 
	/// </summary>
	public class Audio : Document
	{
		public Audio()
		{
			Type = typeof(Audio).Name;
		}
	}
}
