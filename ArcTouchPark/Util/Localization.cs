using System;
using System.Reflection;

namespace ArcTouchPark
{
	public class Localization
	{
		public Localization ()
		{
		}

		public string GetString (string key)
		{
			return Strings.ResourceManager.GetString (key) ?? key;
		}
	}

	public class LocalizationKeyAttribute : Attribute
	{
		public LocalizationKeyAttribute (string key)
		{
			this.Key = key;
		}

		public string Key { get; private set; }

		public static string GetLocalizationKey (object value)
		{
			FieldInfo fieldInfo = value.GetType ().GetRuntimeField (value.ToString ());
			if (fieldInfo == null) {
				return null;
			}

			LocalizationKeyAttribute attribute = fieldInfo.GetCustomAttribute<LocalizationKeyAttribute> ();
			if (attribute == null) {
				return null;
			}

			return attribute.Key;
		}
	}
}

