namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Indicates that the actor has viewed the object.
	/// </summary>
	public class View : Activity
	{
		public View()
		{
			Type = typeof(View).Name;
		}
	}
}
