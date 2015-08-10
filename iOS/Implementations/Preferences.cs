using Foundation;

[assembly: Xamarin.Forms.Dependency (typeof(ArcTouchPark.iOS.Preferences))]
namespace ArcTouchPark.iOS
{
	public class Preferences : IPreferences
	{
		public Preferences ()
		{
		}

		#region implemented abstract members of Settings

		public override void SetString (string key, string value)
		{
			NSUserDefaults.StandardUserDefaults.SetString (value, key);

			NSUserDefaults.StandardUserDefaults.Synchronize ();
		}

		public override string GetString (string key)
		{
			string value = NSUserDefaults.StandardUserDefaults.StringForKey (key);
			return value ?? string.Empty;
		}

		#endregion
	}
}
