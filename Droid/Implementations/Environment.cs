using System;
using System.IO;
using System.Threading.Tasks;

using Android.Content;
using Android.Net;


[assembly: Xamarin.Forms.Dependency (typeof(ArcTouchPark.Droid.Environment))]
namespace ArcTouchPark.Droid
{
	public class Environment : Java.Lang.Object, IEnvironment
	{
		#region Constants

		private const string PNG_MIME_TYPE = "image/png";
		private const string PDF_MIME_TYPE = "application/pdf";
		private const string TEXT_MIME_TYPE = "text/plain";

		#endregion

		public Environment ()
		{
		}

		private string DownloadsFolder {
			get {
				return Path.Combine (Android.OS.Environment.ExternalStorageDirectory.Path, Android.OS.Environment.DirectoryDownloads);
			}
		}

		#region IEnvironment implementation

		public string PersonalFolder {
			get {
				return System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
			}
		}

		public bool IsInternetAvailable {
			get {
				ConnectivityManager connectivityManager = (ConnectivityManager)Xamarin.Forms.Forms.Context.GetSystemService (Android.Content.Context.ConnectivityService);
				var activeConnection = connectivityManager.ActiveNetworkInfo;
				return ((activeConnection != null) && activeConnection.IsConnected);
			}
		}

		#endregion
	}
}