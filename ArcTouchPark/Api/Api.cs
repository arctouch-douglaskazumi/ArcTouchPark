using System;
using System.Threading.Tasks;

namespace ArcTouchPark
{
	public class Api
	{
		private static IPreferences Preferences {
			get {
				return ((App)(App.Current)).Preferences;
			}
		}

		//TODO real auth
		#pragma warning disable 1998
		public static async Task LoginAsync (string username)
		{
			Preferences.SetString (Const.LOGIN_USERNAME, username);
		}

		public static async Task LogoutAsync ()
		{
			Preferences.SetString (Const.LOGIN_USERNAME, string.Empty);
		}
		#pragma warning restore 1998

		public static string GetLoggedUser ()
		{
			return Preferences.GetString (Const.LOGIN_USERNAME);
		}

		public static bool IsLoggedIn ()
		{
			string loggedUser = GetLoggedUser ();
			return loggedUser != string.Empty;
		}
	}
}

