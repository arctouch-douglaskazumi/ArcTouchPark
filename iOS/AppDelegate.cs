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
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();

			ParseClient.Initialize ("J3GQLW4v92nutNykkMdjap0mAuaRlsmZG0QOIB5G", "lmE3U3b5W09XUegOU4aYoNTsq71gXekiFTmwaoIR");

			if (UIDevice.CurrentDevice.CheckSystemVersion (8, 0)) {
				UIUserNotificationType notificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;

				var userNotificationSettings = UIUserNotificationSettings.GetSettingsForTypes (notificationTypes, new NSSet (new string[] { }));
				UIApplication.SharedApplication.RegisterUserNotificationSettings (userNotificationSettings);
				UIApplication.SharedApplication.RegisterForRemoteNotifications ();
			} else {
				var notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
				UIApplication.SharedApplication.RegisterForRemoteNotificationTypes (notificationTypes);
			}

			// Code for starting up the Xamarin Test Cloud Agent
			#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start ();
			#endif

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}

		public override void RegisteredForRemoteNotifications (UIApplication application, NSData deviceToken)
		{
			try {
				var client = new RestClient ("https://api.parse.com");

				var request = new RestRequest ("1/installations/", Method.POST);

				request.AddHeader ("Accept", "application/json");
				request.AddHeader ("X-Parse-Application-Id", "J3GQLW4v92nutNykkMdjap0mAuaRlsmZG0QOIB5G");
				request.AddHeader ("X-Parse-REST-API-Key", "btUxtjBlxay0XWDANgPI0ZnEoK5IcZASNFJOBpdW");
				request.Credentials = new NetworkCredential ("J3GQLW4v92nutNykkMdjap0mAuaRlsmZG0QOIB5G", "ryvMoe9psM2iVwGTP1xCxAcaysCgl3YTIvfMedBA");

				request.Parameters.Clear ();

				string strJSONContent = "{" +
				                        "\"deviceType\":\"ios\"," +
				                        "\"appIdentifier\":\"com.arctouch.park\"," +
				                        "\"channels\":[\"\"]," +
				                        "\"deviceToken\":\"" + deviceToken.Description.Replace ("<", "").Replace (">", "").Replace (" ", "") + "\"" +
				                        "}";
				request.AddParameter ("application/json", strJSONContent, ParameterType.RequestBody);

				client.ExecuteAsync (request, response => {
					Console.WriteLine (response.Content);
				});
			} catch (Exception  ex) {
				Console.WriteLine ("Error registering for notifications in RegisteredForRemoteNotifications! Message: {0}", ex.Message);
			}
		}

		public override void DidRegisterUserNotificationSettings (UIApplication application, UIUserNotificationSettings notificationSettings)
		{
			application.RegisterForRemoteNotifications ();
		}

		public async override void DidReceiveRemoteNotification (UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{
			var objectId = userInfo ["objectId"].ToString ();
			var abdication = await ParseApi.GetAsync<Abdication> (objectId);
			var loggedUser = await Api.GetLoggedUserAsync ();

			if (abdication.Username != loggedUser && abdication.ReplacedByUsername != loggedUser) {
				if (string.IsNullOrWhiteSpace (abdication.ReplacedByUsername)) {
					abdication.ReplacedByUsername = await Api.GetLoggedUserAsync ();
					await ParseApi.SaveAsync (abdication);
					await App.DisplayAlertAsync ("You got the spot");
				} else {
					await App.DisplayAlertAsync ("Spot already taken");
				}
			}
		}
	}
}

