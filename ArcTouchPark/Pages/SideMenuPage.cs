using System;
using System.Reflection;

using Xamarin.Forms;

namespace ArcTouchPark
{
	public class SideMenuPage : ContentPage
	{
		#region Design Constants

		private static readonly Color BACKGROUND_COLOR = VisualDesign.BRAND_COLOR;

		private static readonly Color HEADING_LABEL_TEXT_COLOR = Color.White;
		private const double HEADING_LABEL_FONT_SIZE = 16;

		private const double LAYOUT_SPACING = 10;
		private static readonly Thickness LAYOUT_PADDING = new Thickness (20, 10);

		#endregion

		private StackLayout layout;

		public SideMenuPage ()
		{
			BackgroundColor = BACKGROUND_COLOR;
			Title = Localization.GetString ("AppName");

			CreateLayout ();
		}

		#region Events

		public event EventHandler<SideMenuItem> ItemTapped;

		protected internal virtual void OnItemTapped (object sender, SideMenuItem sideMenuItem)
		{
			EventHandler<SideMenuItem> eventHandler = ItemTapped;
			if (eventHandler != null) {
				eventHandler (this, sideMenuItem);
			}
		}

		#endregion

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			foreach (View view in this.layout.Children) {
				Button button = view as Button;
				if (button != null) {
					button.Clicked += SideMenuItemTapped;
				}
			}
		}

		protected override void OnDisappearing ()
		{
			foreach (View view in this.layout.Children) {
				Button button = view as Button;
				if (button != null) {
					button.Clicked -= SideMenuItemTapped;
				}
			}

			base.OnDisappearing ();
		}

		private Localization Localization {
			get {
				return ((App)(App.Current)).Localization;
			}
		}

		private void CreateLayout ()
		{
			Label headingLabel = new Label () {
				TextColor = HEADING_LABEL_TEXT_COLOR,
				HorizontalOptions = LayoutOptions.Fill,
				XAlign = TextAlignment.Center,
				YAlign = TextAlignment.Center,
				Text = Localization.GetString ("AppName"),
				FontFamily = VisualDesign.FONT_FAMILY_MEDIUM,
				FontSize = VisualDesign.ScaleFontSize (HEADING_LABEL_FONT_SIZE)
			};

			Thickness layoutPadding = VisualDesign.ScalePadding (LAYOUT_PADDING);
			layoutPadding.Top += VisualDesign.STATUS_BAR_OFFSET_UNSCALED;
			this.layout = new StackLayout () {
				Orientation = StackOrientation.Vertical,
				Spacing = VisualDesign.ScaleHeight (LAYOUT_SPACING),
				Padding = layoutPadding,
				Children = {
					headingLabel
				}
			};

			foreach (SideMenuItem sideMenuItem in Enum.GetValues(typeof(SideMenuItem))) {
				if (sideMenuItem == SideMenuItem.None) {
					continue;
				}

				Button button = VisualDesign.CreateSecondaryButton (Localization.GetString (LocalizationKeyAttribute.GetLocalizationKey (sideMenuItem)));
				button.CommandParameter = sideMenuItem;
				this.layout.Children.Add (button);
			}

			Content = this.layout;
		}

		#region Event Handlers

		private void SideMenuItemTapped (object sender, EventArgs e)
		{
			EventHandler<SideMenuItem> handler = ItemTapped;
			if (handler != null) {
				SideMenuItem sideMenuItem = (sender is Button) ? (SideMenuItem)((Button)sender).CommandParameter : SideMenuItem.None;
				handler (this, sideMenuItem);
			}
		}

		#endregion
	}

	public enum SideMenuItem
	{
		None = 0,
		[LocalizationKey ("Home"), Page (typeof(MainPage))]
		Home = 1,
		[LocalizationKey ("Logout"), Page (typeof(MainPage))]
		Logout = 2
	}

	public class PageAttribute : Attribute
	{
		public PageAttribute (Type type)
		{
			this.Type = type;
		}

		public Type Type { get; private set; }

		public static Page GetPage (object value)
		{
			FieldInfo fieldInfo = value.GetType ().GetRuntimeField (value.ToString ());
			if (fieldInfo == null) {
				return null;
			}

			PageAttribute attribute = fieldInfo.GetCustomAttribute<PageAttribute> ();
			if (attribute == null) {
				return null;
			}

			return (Page)Activator.CreateInstance (attribute.Type);
		}
	}
}

