using System;

using Xamarin.Forms;

namespace ArcTouchPark
{
	public static class VisualDesign
	{
		public static readonly int STATUS_BAR_OFFSET_UNSCALED = (Device.OS == TargetPlatform.iOS) ? 20 : 0;

		public static readonly Color BRAND_COLOR = Color.FromRgb (0xFF, 0x83, 0x00);
		public static readonly Color BACKGROUND_COLOR = Color.FromRgb (0x55, 0x56, 0x5A);

		public static readonly string FONT_FAMILY = Device.OnPlatform ("Gotham-Book", "Gotham-Book.ttf", "Gotham-Book");
		public static readonly string FONT_FAMILY_MEDIUM = Device.OnPlatform ("Gotham-Medium", "Gotham-Medium.ttf", "Gotham-Medium");

		public static readonly Color PRIMARY_BUTTON_BACKGROUND_COLOR = BRAND_COLOR;
		public static readonly Color PRIMARY_BUTTON_TEXT_COLOR = Color.White;
		public const double PRIMARY_BUTTON_FONT_SIZE = 14.0;
		public static readonly string PRIMARY_BUTTON_FONT_FAMILY = FONT_FAMILY;

		private static readonly Color SECONDARY_BUTTON_BACKGROUND_COLOR = BACKGROUND_COLOR;
		private static readonly Color SECONDARY_BUTTON_TEXT_COLOR = Color.White;
		public const double SECONDARY_BUTTON_FONT_SIZE = 14.0;
		public static readonly string SECONDARY_BUTTON_FONT_FAMILY = FONT_FAMILY;

		public const double LABEL_FONT_SIZE_BIG = 18.0;
		public const double LABEL_FONT_SIZE_SMALL = 14.0;

		private const double TARGET_DESIGN_WIDTH = 768;
		private const double TARGET_DESIGN_HEIGHT = 1024;

		static VisualDesign ()
		{
			VisualDesign.DeviceBounds = new Size (TARGET_DESIGN_WIDTH, TARGET_DESIGN_HEIGHT);
		}

		public static void Initialize (double deviceWidth, double deviceHeight)
		{
			VisualDesign.DeviceBounds = new Size (deviceWidth, deviceHeight);
		}

		public static Size DeviceBounds { get; private set; }

		public static double DeviceWidth {
			get {
				return DeviceBounds.Width;
			}
		}

		public static double DeviceHeight {
			get {
				return DeviceBounds.Height;
			}
		}

		public static bool IsPortrait {
			get {
				return (DeviceHeight > DeviceWidth);
			}
		}

		public static double TargetDesignWidth {
			get {
				return IsPortrait ? TARGET_DESIGN_WIDTH : TARGET_DESIGN_HEIGHT;
			}
		}

		public static double TargetDesignHeight {
			get {
				return IsPortrait ? TARGET_DESIGN_HEIGHT : TARGET_DESIGN_WIDTH;
			}
		}

		public static bool IsTablet {
			get {
				return (Device.Idiom == TargetIdiom.Tablet);
			}
		}

		public static Button CreatePrimaryButton (string text = null)
		{
			return new Button () {
				BackgroundColor = PRIMARY_BUTTON_BACKGROUND_COLOR,
				TextColor = PRIMARY_BUTTON_TEXT_COLOR,
				Text = text,
				FontSize = ScaleFontSize (PRIMARY_BUTTON_FONT_SIZE),
				FontFamily = PRIMARY_BUTTON_FONT_FAMILY
			};
		}

		public static Button CreateSecondaryButton (string text = null)
		{
			return new Button () {
				BackgroundColor = SECONDARY_BUTTON_BACKGROUND_COLOR,
				TextColor = SECONDARY_BUTTON_TEXT_COLOR,
				Text = text,
				FontSize = ScaleFontSize (SECONDARY_BUTTON_FONT_SIZE),
				FontFamily = SECONDARY_BUTTON_FONT_FAMILY
			};
		}

		#region Scaling Methods

		public static double ScaleWidth (double width)
		{
			return Math.Round ((width / TargetDesignWidth) * DeviceWidth);
		}

		public static double ScaleHeight (double height)
		{
			return Math.Round ((height / TargetDesignHeight) * DeviceHeight);
		}

		public static Thickness ScalePadding (Thickness thickness)
		{
			return new Thickness (VisualDesign.ScaleWidth (thickness.Left), VisualDesign.ScaleHeight (thickness.Top),
				VisualDesign.ScaleWidth (thickness.Right), VisualDesign.ScaleHeight (thickness.Bottom));
		}

		public static double ScaleFontSize (double fontSize)
		{
			return ScaleHeight (fontSize);
		}

		#endregion
	}
}

