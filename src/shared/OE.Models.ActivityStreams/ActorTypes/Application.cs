namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Describes a software application. 
	/// </summary>
	public class Application : ActorBase
	{
		public Application()
		{
			Type = typeof(Application).Name;
		}
	}
}
