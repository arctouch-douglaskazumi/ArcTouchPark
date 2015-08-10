using System;
using System.Globalization;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ArcTouchPark
{
	public class VisualDesignExtension : IMarkupExtension
	{
		public VisualDesignExtension ()
		{
		}

		public double ScaleWidth { get; set; }

		public double ScaleHeight { get; set; }

		public Thickness ScalePadding { get; set; }

		public double ScaleFontSize { get; set; }

		#region IMarkupExtension implementation

		public object ProvideValue (IServiceProvider serviceProvider)
		{
			if (ScaleWidth > 0) {
				return VisualDesign.ScaleWidth (ScaleWidth);
			} else if (ScaleHeight > 0) {
				return VisualDesign.ScaleHeight (ScaleHeight);
			} else if (ScalePadding != default(Thickness)) {
				return VisualDesign.ScalePadding (ScalePadding);
			} else if (ScaleFontSize > 0) {
				return VisualDesign.ScaleFontSize (ScaleFontSize);
			}

			return null;
		}

		#endregion
	}
}
