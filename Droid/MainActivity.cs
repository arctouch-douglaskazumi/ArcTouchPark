using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Parse;

namespace ArcTouchPark.Droid
{
	[Activity (Label = "ArcTouchPark.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			ParseClient.Initialize ("J3GQLW4v92nutNykkMdjap0mAuaRlsmZG0QOIB5G", "lmE3U3b5W09XUegOU4aYoNTsq71gXekiFTmwaoIR");

			global::Xamarin.Forms.Forms.Init (this, bundle);

			LoadApplication (new App ());
		}
	}
}

