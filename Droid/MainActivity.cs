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
	[Activity (
		Label = "ArcTouch Park", 
		Icon = "@drawable/icon", 
		MainLauncher = true, 
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
		Theme = "@style/NoIcon")]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
			var app = new App ();
			LoadApplication (app);

			string objectId = Intent.GetStringExtra (Const.OBJECT_ID);
			if (!string.IsNullOrWhiteSpace (objectId)) {
				app.OnNotificationReceived (objectId);
			}
		}
	}
}

