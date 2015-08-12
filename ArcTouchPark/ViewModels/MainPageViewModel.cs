using System;
using System.Windows.Input;
using Xamarin.Forms;

using System.Collections.Generic;

namespace ArcTouchPark
{
	public class MainPageViewModel : ViewModelBase
	{
		public MainPageViewModel ()
		{
			Initialize ();
		}

		private Localization Localization {
			get {
				return ((App)(App.Current)).Localization;
			}
		}

		protected async void Initialize ()
		{
			SelectedDate = DateTime.Now;
			WontUseCommand = new Command (WontUse);
			LogoutNotHereCommand = new Command (LogoutNotHere);
			Username = await Api.GetLoggedUserAsync () ?? Localization.GetString ("Logout");
		}

		#region Properties

		private DateTime selectedDate;

		public DateTime SelectedDate {
			get {
				return this.selectedDate;
			}

			set {
				if (this.selectedDate != value) {
					this.selectedDate = value;
					RaisePropertyChanged ();
				}
			}
		}

		private string username;

		public string Username {
			get {
				return this.username;
			}

			set {
				if (this.username != value) {
					this.username = value;
					RaisePropertyChanged ();
				}
			}
		}

		#endregion

		#region Commands

		public ICommand WontUseCommand { get; private set; }

		private async void WontUse ()
		{
			string message = Localization.GetString ("SuccessfullAbdication");
			try {
				Abdication abdication = new Abdication ();
				abdication.Username = Username;
				abdication.SelectedDate = SelectedDate;
				await ParseApi.SaveAsync (abdication);
			} catch (Exception e) {
				message = Localization.GetString ("OopsSomethingWrong");
			}

			await App.DisplayAlertAsync (message);
		}

		public ICommand LogoutNotHereCommand { get; private set; }

		private async void LogoutNotHere ()
		{
			var wrongPlaceMessage = Localization.GetString ("LogoutNotHere");
			await App.DisplayAlertAsync (wrongPlaceMessage);
		}

		#endregion
	}
}

