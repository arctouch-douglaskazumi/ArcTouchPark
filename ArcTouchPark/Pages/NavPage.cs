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

		public NavPage ()
		{
			MasterBehavior = MasterBehavior.Default;

			this.sideMenuPage = new SideMenuPage ();
			this.navPage = new NavigationPage (new MainPage ()) {
				BarBackgroundColor = VisualDesign.BACKGROUND_COLOR,
				BarTextColor = VisualDesign.BRAND_COLOR
			};

			Master = sideMenuPage;
			Detail = navPage;

		}

		#region Navigation

		public Task PushPageAsync (Page page)
		{
			return this.navPage.PushAsync (page);
		}

		public Task<Page> PopPageAsync ()
		{
			return this.navPage.PopAsync ();
		}

		public Task PopToRootAsync (bool animate = true)
		{
			return this.navPage.PopToRootAsync (animate);
		}

		public async Task NavigateToPage (SideMenuItem sideMenuItem)
		{
			IsPresented = false;

			switch (sideMenuItem) {
			// FIXIT: Enable again when Dashboard is created
			//case SideMenuItem.Home:
			//    await PopToRootAsync(false);
			//    break;
			default:
				Page page = PageAttribute.GetPage (sideMenuItem);
				if (!IsSameAsCurrentPage (page)) {
					await PopToRootAsync (false);
					await PushPageAsync (page);
				}
				break;
			}

			App.OnSideMenuItemChanged (sideMenuItem);

			this.sideMenuPage.ForceLayout ();
		}

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
			Task popTask = PopToRootAsync ();
			Task pushTask = null;
			switch (sideMenuItem) {
			case SideMenuItem.Home:
				// Root is already HomePage
				break;
			case SideMenuItem.Logout:
				await LogOut ();
				break;
			default:
				pushTask = PushPageAsync (PageAttribute.GetPage (sideMenuItem));
				break;
			}

			if (pushTask != null) {
				await Task.WhenAll (popTask, pushTask);
			} else {
				await popTask;
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


