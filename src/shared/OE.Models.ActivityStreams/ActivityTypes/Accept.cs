namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Indicates that the actor accepts the object. The target property can be used in certain
	/// circumstances to indicate the context into which the object has been accepted. 
	/// </summary>
	public class Accept : Activity
	{
		public Accept()
		{
			Type = typeof(Accept).Name;
		}
	}
}
