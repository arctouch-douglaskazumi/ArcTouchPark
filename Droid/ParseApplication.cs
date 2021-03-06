﻿using System;
using Android.App;

using Parse;
using Android.Runtime;
using Android.Content;

namespace ArcTouchPark.Droid
{
	[Application (Name = "arctouchpark.droid.ParseApplication")]
	public class ParseApplication: Application
	{
		private static int notificationId = 0;

		public ParseApplication (IntPtr handle, JniHandleOwnership ownerShip)
			: base (handle, ownerShip)
		{
		}

		private Localization Localization {
			get {
				return ((App)(App.Current)).Localization;
			}
		}

		public async override void OnCreate ()
		{
			base.OnCreate ();
			ParseClient.Initialize (Const.APP_ID, Const.DOT_NET_KEY);
			await ParsePush.SubscribeAsync ("");
			ParsePush.ParsePushNotificationReceived += PushNotificationHandler;

		}

		public void PushNotificationHandler (object sender, ParsePushNotificationEventArgs args)
		{
			var payload = args.Payload;
			object objectId;
			object alertMessage;
			if (payload.TryGetValue (Const.OBJECT_ID, out objectId)) {
				if (payload.TryGetValue (Const.ALERT_DICT_KEY, out alertMessage)) {
					generateNotification (alertMessage.ToString (), (string)objectId);
				}
			}
		}

		private void generateNotification (string message, string objectId)
		{
			Intent intent = new Intent (this, typeof(MainActivity));
			intent.PutExtra (Const.OBJECT_ID, objectId);

			PendingIntent pendingIntent = PendingIntent.GetActivity (this, notificationId++, intent, PendingIntentFlags.OneShot);

			Notification.Builder builder = new Notification.Builder (this)
				.SetContentIntent (pendingIntent)
				.SetContentTitle (Localization.GetString (Const.APP_NAME))
				.SetContentText (message)
				.SetSmallIcon (Resource.Drawable.icon);

			Notification.BigTextStyle longText = new Notification.BigTextStyle (builder);
			longText.BigText (message);
			longText.SetSummaryText (GetString ("HurryUp"));

			Notification notification = builder.Build ();
			notification.Flags = NotificationFlags.AutoCancel;

			NotificationManager notificationManager = GetSystemService (Context.NotificationService) as NotificationManager;

			notificationManager.Notify (notificationId, notification);
		}

	}
}

