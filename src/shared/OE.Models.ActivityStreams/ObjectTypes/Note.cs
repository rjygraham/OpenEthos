namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Represents a short written work typically less than a single paragraph in length. 
	/// </summary>
	public class Note : ActivityStreamsObject
	{
		public Note()
		{
			Type = typeof(Note).Name;
		}
	}
}
