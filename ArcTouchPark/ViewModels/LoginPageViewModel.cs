using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace ArcTouchPark
{
	public class LoginPageViewModel : ViewModelBase
	{
		public LoginPageViewModel ()
		{
			Initialize ();
		}

		public App App {
			get {
				return (App)Application.Current;
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

		protected void Initialize ()
		{
			Username = string.Empty;
			LoginCommand = new Command (Login);
		}

		public ICommand LoginCommand { get; private set; }

		private async void Login ()
		{
			if (!string.IsNullOrWhiteSpace (username)) {
				await Api.LoginAsync (username);
				App.MainPage = new NavPage ();
			}
		}
	}
}

