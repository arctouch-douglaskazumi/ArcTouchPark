using Android.Content;
using Android.Preferences;

[assembly: Xamarin.Forms.Dependency (typeof(ArcTouchPark.Droid.Preferences))]
namespace ArcTouchPark.Droid
{
	public class Preferences : IPreferences
	{
		public Preferences ()
		{
		}

		#region ISettings implementation

		public override void SetString (string key, string value)
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences (Xamarin.Forms.Forms.Context); 
			ISharedPreferencesEditor editor = prefs.Edit ();

			editor.PutString (key, value);

			editor.Apply ();
		}

		public override string GetString (string key)
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences (Xamarin.Forms.Forms.Context); 

			if (!prefs.Contains (key)) {
				return "";
			}

			return prefs.GetString (key, "");
		}

		#endregion
	}
}
