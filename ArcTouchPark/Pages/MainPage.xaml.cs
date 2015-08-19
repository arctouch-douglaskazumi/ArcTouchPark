using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Threading.Tasks;

namespace ArcTouchPark
{
	public partial class MainPage : ContentPage
	{
		private MainPageViewModel mainPageViewModel;
		private bool isNotificationHandlerAttached;

		public App app {
			get {
				return ((App)App.Current);
			}
		}

		public MainPage ()
		{
			this.mainPageViewModel = new MainPageViewModel ();
			BindingContext = this.mainPageViewModel;
			InitializeComponent ();
			app.NotificationReceived += NotificationReceived;
			isNotificationHandlerAttached = true;
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			if (!isNotificationHandlerAttached) {
				app.NotificationReceived += NotificationReceived;
				isNotificationHandlerAttached = true;
			}
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
			app.NotificationReceived -= NotificationReceived;
			isNotificationHandlerAttached = false;
		}

		private void NotificationReceived (object sender, string objectId)
		{
			mainPageViewModel.NotificationClickedAsync (objectId);
		}
	}
}

