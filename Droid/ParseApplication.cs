using System;
using Android.App;

using Parse;
using Android.Runtime;

namespace ArcTouchPark.Droid
{
	[Application (Name = "arctouchpark.droid.ParseApplication")]
	public class ParseApplication: Application
	{
		public ParseApplication (IntPtr handle, JniHandleOwnership ownerShip)
			: base (handle, ownerShip)
		{
		}

		public async override void OnCreate ()
		{
			try {
				base.OnCreate ();
				ParseClient.Initialize ("J3GQLW4v92nutNykkMdjap0mAuaRlsmZG0QOIB5G", "lmE3U3b5W09XUegOU4aYoNTsq71gXekiFTmwaoIR");
				await ParsePush.SubscribeAsync ("");
				ParsePush.ParsePushNotificationReceived += (sender, args) => {
					var payload = args.Payload;
					object objectId;
					if (payload.TryGetValue ("objectId", out objectId)) {
						App.DisplayAlertAsync ("ola");
					}
				};
			} catch (Exception ex) {
				ex = ex;
			}
		}
	}
}

