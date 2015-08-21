using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Parse;
using UIKit;
using RestSharp;
using System.Net;

using Newtonsoft.Json;

namespace ArcTouchPark.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		private App parkApp;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();

			ParseClient.Initialize (Const.APP_ID, Const.DOT_NET_KEY);

			SubsribeToPushVersionSafe ();

			// Code for starting up the Xamarin Test Cloud Agent
			#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start ();
			#endif

			parkApp = new App ();
			LoadApplication (parkApp);

			return base.FinishedLaunching (app, options);
		}

		static void SubsribeToPushVersionSafe ()
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion (8, 0)) {
				UIUserNotificationType notificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
				var userNotificationSettings = UIUserNotificationSettings.GetSettingsForTypes (notificationTypes, new NSSet (new string[] {

				}));
				UIApplication.SharedApplication.RegisterUserNotificationSettings (userNotificationSettings);
				UIApplication.SharedApplication.RegisterForRemoteNotifications ();
			} else {
				var notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
				UIApplication.SharedApplication.RegisterForRemoteNotificationTypes (notificationTypes);
			}
		}

		public async override void RegisteredForRemoteNotifications (UIApplication application, NSData deviceToken)
		{
			ParseInstallation installation = ParseInstallation.CurrentInstallation;
			installation.SetDeviceTokenFromData (deviceToken);
			await ParsePush.SubscribeAsync ("");
			await installation.SaveAsync ();
		}

		public override void DidRegisterUserNotificationSettings (UIApplication application, UIUserNotificationSettings notificationSettings)
		{
			application.RegisterForRemoteNotifications ();
		}

		public override void ReceivedRemoteNotification (UIApplication application, NSDictionary userInfo)
		{
			ParsePush.HandlePush (userInfo);
		}

		public override void DidReceiveRemoteNotification (UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{
			var message = (userInfo.ObjectForKey (new NSString (Const.APS_DICT_KEY)) as NSDictionary) [Const.ALERT_DICT_KEY].ToString ();
			if (application.ApplicationState != UIApplicationState.Active || message.Contains ("parking spot")) { //TODO needs a better way to identify pushes that need reply
				var objectId = userInfo [Const.OBJECT_ID].ToString ();
				parkApp.OnNotificationReceived (objectId);
			} else {
				if (!string.IsNullOrWhiteSpace (message)) {
					App.DisplayAlertAsync (message);
				}	
			}
		}
	}
}

