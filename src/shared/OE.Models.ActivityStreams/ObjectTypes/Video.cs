namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Represents a video document of any kind. 
	/// </summary>
	public class Video : Document
	{
		public Video()
		{
			Type = typeof(Video).Name;
		}
	}
}
