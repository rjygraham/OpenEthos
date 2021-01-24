namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Represents a document of any kind. 
	/// </summary>
	public class Document : ActivityStreamsObject
	{
		public Document()
		{
			Type = typeof(Document).Name;
		}
	}
}
