using System;
using System.Threading.Tasks;

namespace ArcTouchPark
{
	public class Api
	{
		public const string LOGIN_USERNAME = "Login";

		private static IPreferences Preferences {
			get {
				return ((App)(App.Current)).Preferences;
			}
		}

		public static async Task LoginAsync (string username)
		{
			Preferences.SetString (LOGIN_USERNAME, username);
		}

		public static async Task LogoutAsync ()
		{
			Preferences.SetString (LOGIN_USERNAME, string.Empty);
		}

		public static async Task<string> GetLoggedUserAsync ()
		{
			return Preferences.GetString (LOGIN_USERNAME);
		}

		public static async Task<bool> IsLoggedInAsync ()
		{
			string loggedUser = await GetLoggedUserAsync ();
			return loggedUser != string.Empty;
		}
	}
}

