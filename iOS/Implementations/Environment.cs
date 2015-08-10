using System;
using System.IO;
using System.Threading.Tasks;

using UIKit;
using Foundation;


[assembly: Xamarin.Forms.Dependency (typeof(ArcTouchPark.iOS.Environment))]
namespace ArcTouchPark.iOS
{

	public class Environment : IEnvironment
	{

		public Environment ()
		{
		}

		#region IEnvironment implementation

		public string PersonalFolder {
			get {
				return System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
			}
		}

		public bool IsInternetAvailable {
			get {
				return Reachability.IsHostReachable ("www.google.com");
			}
		}

		#endregion

		private UIView GetShareDialogForTablet ()
		{
			// TODO: Try to change it later to find the corresponding UIView, so we wont need these magical numbers
			nfloat scale = UIApplication.SharedApplication.KeyWindow.Screen.NativeScale;

			double x = 0;
			double y = 0;

			switch (UIApplication.SharedApplication.StatusBarOrientation) {
			case UIInterfaceOrientation.Portrait:
				x = 0.92;
				y = 0.05;
				break;
			case UIInterfaceOrientation.PortraitUpsideDown:
				x = 0.08;
				y = 0.95;
				break;
			case UIInterfaceOrientation.LandscapeRight:
				x = 0.85;
				y = 1.25;
				break;
			case UIInterfaceOrientation.LandscapeLeft:
				x = 0.02;
				y = 0.08;
				break;
			default:
				break;
			}

			x *= VisualDesign.DeviceWidth * scale;
			y *= VisualDesign.DeviceHeight * scale;
			return new UIView (new CoreGraphics.CGRect (x, y, 0, 0));
		}

		public void ExcludeFileFromBackup (String filePath)
		{
			NSFileManager.SetSkipBackupAttribute (filePath, true);
		}
	}
}