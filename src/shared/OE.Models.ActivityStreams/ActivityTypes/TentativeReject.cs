namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// A specialization of Reject in which the rejection is considered tentative. 
	/// </summary>
	public class TentativeReject : Reject
	{
		public TentativeReject()
		{
			Type = typeof(TentativeReject).Name;
		}
	}
}
