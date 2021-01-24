namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// Represents any kind of multi-paragraph written work.
	/// </summary>
	public class Article : ActivityStreamsObject
	{
		public Article()
		{
			Type = typeof(Article).Name;
		}
	}
}
