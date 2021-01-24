namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Indicates that the actor has added the object to the target. If the target property is not
	/// explicitly specified, the target would need to be determined implicitly by context. The 
	/// origin can be used to identify the context from which the object originated. 
	/// </summary>
	public class Add : Activity
	{
		public Add()
		{
			Type = typeof(Add).Name;
		}
	}
}
