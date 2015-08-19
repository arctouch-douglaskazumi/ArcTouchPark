using System;
using System.Windows.Input;
using Xamarin.Forms;

using System.Collections.Generic;
using System.Threading.Tasks;

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

		protected void Initialize ()
		{
			SelectedDate = DateTime.Now;
			WontUseCommand = new Command (WontUse);
			LogoutNotHereCommand = new Command (LogoutNotHere);
			Username = Api.GetLoggedUser () ?? Strings.Logout;
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
			string message = Strings.SuccessfullAbdication;
			try {
				Abdication abdication = new Abdication ();
				abdication.Username = Username;
				abdication.SelectedDate = SelectedDate;
				IsRunning = true;
				await ParseApi.SaveAsync (abdication);
			} catch (Exception e) {
				message = Strings.OopsSomethingWrong;
			} finally {
				IsRunning = false;
			}

			await App.DisplayAlertAsync (message);
		}

		public ICommand LogoutNotHereCommand { get; private set; }

		private async void LogoutNotHere ()
		{
			var wrongPlaceMessage = Strings.LogoutNotHere;
			await App.DisplayAlertAsync (wrongPlaceMessage);
		}

		#endregion

		public async Task NotificationClicked (string objectId)
		{
			IsRunning = true;
			try {
				var abdication = await ParseApi.GetAsync<Abdication> (objectId);

				var onlyInfo = !string.IsNullOrWhiteSpace (abdication.Username) && !string.IsNullOrWhiteSpace (abdication.ReplacedByUsername);
				if (onlyInfo)
					return;

				var formattedMessage = string.Format (Strings.WantToGetSpot, abdication.Username, abdication.SelectedDate.ToString ("d"));
				bool getSpot = await App.DisplayYesNoDialogAsync (formattedMessage);

				if (getSpot) {
					var loggedUser = Api.GetLoggedUser ();
					string message = Strings.SpotAlreadyTaken;

					if (string.IsNullOrWhiteSpace (abdication.ReplacedByUsername)) {
						abdication.ReplacedByUsername = loggedUser;

						await ParseApi.SaveAsync (abdication);

						message = Strings.YouGotTheSpot;
					}

					await App.DisplayAlertAsync (message);
				}
			} catch (Exception ex) {
				App.DisplayAlert (ex.Message);
			} finally {
				IsRunning = false;
			}
		}

	}
}

