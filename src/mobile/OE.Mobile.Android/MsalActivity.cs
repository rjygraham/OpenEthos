using Android.App;
using Android.Content;
using Android.OS;
using Microsoft.Identity.Client;

namespace OE.Mobile.Droid
{
	[Activity]
	[IntentFilter(new[] { Intent.ActionView },
		Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
		DataHost = "com.companyname.openethos",
		DataScheme = "auth")]
	public class MsalActivity : BrowserTabActivity
	{
	}
}