using OE.Mobile.Services;
using Plugin.CurrentActivity;

namespace OE.Mobile.Droid.Services
{
	public class AndroidParentWindowLocatorService : IParentWindowLocatorService
	{
		public object GetCurrentParentWindow()
		{
			return CrossCurrentActivity.Current.Activity;
		}
	}
}