using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Microsoft.Identity.Client;

namespace OE.Mobile.Droid
{
	[Activity]
	[IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault }, DataHost = "co.getjust.openethos", DataScheme = "auth")]
	public class MsalActivity : BrowserTabActivity
	{
		protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
		}
	}
}