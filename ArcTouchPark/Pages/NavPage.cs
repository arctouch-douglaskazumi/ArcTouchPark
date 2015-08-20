using System;

using Xamarin.Forms;
using System.Threading.Tasks;

namespace ArcTouchPark
{
	public class NavPage : MasterDetailPage
	{
		private SideMenuPage sideMenuPage;
		private NavigationPage navPage;

		public App App {
			get {
				return (App)Application.Current;
			}
		}

		private Localization Localization {
			get {
				return ((App)(App.Current)).Localization;
			}
		}

		private NavigationPage StyledNavigationPage (Page newPage)
		{
			return new NavigationPage (newPage) {
				BarBackgroundColor = VisualDesign.BACKGROUND_COLOR,
				BarTextColor = VisualDesign.BRAND_COLOR,
				BackgroundColor = VisualDesign.BACKGROUND_COLOR
			};
		}

		public NavPage ()
		{
			MasterBehavior = MasterBehavior.Popover;

			this.sideMenuPage = new SideMenuPage ();
			this.sideMenuPage.Icon = "menu.png";

			Master = sideMenuPage;

			var homePage = new MainPage ();
			homePage.Title = Localization.GetString (LocalizationKeyAttribute.GetLocalizationKey (SideMenuItem.Home));
			this.navPage = StyledNavigationPage (homePage);

			Detail = navPage;

		}

		#region Navigation

		private bool IsSameAsCurrentPage (Page page)
		{
			return (this.navPage.CurrentPage != null) && (this.navPage.CurrentPage.GetType () == page.GetType ());
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			this.sideMenuPage.ItemTapped += SideMenuItemTapped;

			CheckForAuth ();
		}

		protected override void OnDisappearing ()
		{
			this.sideMenuPage.ItemTapped -= SideMenuItemTapped;

			base.OnDisappearing ();
		}

		#endregion


		#region Event Handlers

		private async void SideMenuItemTapped (object sender, SideMenuItem sideMenuItem)
		{
			IsPresented = false;

			switch (sideMenuItem) {
			case SideMenuItem.Logout:
				await LogOut ();
				break;
			default:
				var newPage = PageAttribute.GetPage (sideMenuItem);
				if (!IsSameAsCurrentPage (newPage)) {
					newPage.Title = Localization.GetString (LocalizationKeyAttribute.GetLocalizationKey (sideMenuItem));
					navPage = StyledNavigationPage (newPage);

					Detail = navPage;
				}
				break;
			}
		}

		#endregion

		public void CheckForAuth ()
		{
			bool isLoggedIn = Api.IsLoggedIn ();
			if (!isLoggedIn) {
				ShowLoginPage ();
			}
		}

		public async Task LogOut ()
		{
			IsPresented = false;

			await Api.LogoutAsync ();
			ShowLoginPage ();
		}

		private void ShowLoginPage ()
		{			
			App.MainPage = new LoginPage ();
		}
	}
}


