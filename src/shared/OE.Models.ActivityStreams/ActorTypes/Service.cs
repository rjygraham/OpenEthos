namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Represents a service of any kind. 
	/// </summary>
	public class Service : ActorBase
	{
		public Service()
		{
			Type = typeof(Service).Name;
		}
	}
}
