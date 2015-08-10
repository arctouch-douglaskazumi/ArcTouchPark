using System;

namespace ArcTouchPark
{
	public abstract class IPreferences
	{
		public const string EMAIL_ADDRESS = "EMAIL_ADDRESS";
		public const string PASSWORD = "PASSWORD";

		public abstract string GetString (string key);

		public abstract void SetString (string key, string value);
	}
}