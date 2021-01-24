namespace OE.Models.ActivityStreams
{
	/// <summary>
	/// A specialization of Accept indicating that the acceptance is tentative. 
	/// </summary>
	public class TentativeAccept : Accept
	{
		public TentativeAccept()
		{
			Type = typeof(TentativeAccept).Name;
		}
	}
}
