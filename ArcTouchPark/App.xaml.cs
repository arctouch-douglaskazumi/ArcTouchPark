using System;

using Xamarin.Forms;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;

namespace ArcTouchPark
{
	public partial class App : Application
	{
		public ILogging Logging { get; private set; }

		public IPreferences Preferences { get; private set; }

		public IEnvironment Environment { get; private set; }

		public Localization Localization { get; private set; }

		public App ()
		{
			InitializeComponent ();
			Localization = new Localization ();
			Preferences = DependencyService.Get<IPreferences> ();

			MainPage = new ContentPage ();
		}

		protected override void OnStart ()
		{
			MainPage = new NavPage ();
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

		public static async Task DisplayAlertAsync (string message, string title = null)
		{
			await Application.Current.MainPage.DisplayAlert (title ?? Strings.AppName, message, Strings.OK);
		}

		public static async Task<bool> DisplayYesNoDialogAsync (string message, string title = null)
		{
			return await DisplayBinaryDialogAsync (message, title, Strings.Yes, Strings.No);
		}

		public static async Task<bool> DisplayBinaryDialogAsync (string message, string title = null, params string[] options)
		{
			return await Application.Current.MainPage.DisplayAlert (title ?? Strings.AppName, message, options [0], options [1]);
		}

		public event EventHandler<SideMenuItem> SideMenuItemChanged;

		public void OnSideMenuItemChanged (SideMenuItem sideMenuItem)
		{
			EventHandler<SideMenuItem> handler = SideMenuItemChanged;
			if (handler != null) {
				handler (this, sideMenuItem);
			}
		}

		public event EventHandler<string> NotificationReceived;

		public async void OnNotificationReceived (string objectId)
		{
			EventHandler<string> handler = NotificationReceived;
			if (handler != null) {
				handler (this, objectId);
			}
		}

		public static void HandleException (string message, Exception exception)
		{
			if (exception != null) {
				App app = (App)App.Current;

				if ((exception is WebException) && !App.IsInternetAvailable) {
					#if DEBUG
					app.Logging.Log (String.Format ("Ignoring: {0} (Offline)", message)); 
					#endif   
					return;
				}
			}

			DisplayAlert (message);
		}

		public static void DisplayAlert (string message, string title = null)
		{
			Device.BeginInvokeOnMainThread (() => DisplayAlertAsync (message, title));
		}

		#region Static Accessor Methods

		public static bool IsInternetAvailable { 
			get { 
				App app = (App)App.Current;
				return app.Environment.IsInternetAvailable;
			} 
		}

		public static async Task PushReceiver ()
		{
			await DisplayAlertAsync ("message");
		}


		#endregion
	}
}

