using System.Threading.Tasks;

using Xamarin.Forms;

namespace ArcTouchPark
{
	public class MainPage : MasterDetailPage
	{
		#region Design Constants

		private static readonly Color NAVIGATION_BAR_TEXT_COLOR = Colors.WHITE;
		private static readonly Color NAVIGATION_BAR_BACKGROUND_COLOR = Colors.ENDEAVOUR;

		#endregion

		private SideMenuPage sideMenuPage;
		private NavigationPage navigationPage;

		public MainPage ()
		{
			this.sideMenuPage = new SideMenuPage ();
			this.navigationPage = new SearchableNavigationPage () {
				BarTextColor = NAVIGATION_BAR_TEXT_COLOR,
				BarBackgroundColor = NAVIGATION_BAR_BACKGROUND_COLOR
			};

			Master = this.sideMenuPage;
			Detail = this.navigationPage;
			MasterBehavior = MasterBehavior.Popover; // Tablet and Phone have the same behavior
		}

		#region Navigation

		public Task PushPageAsync (Page page)
		{
			AdjustLayout (page);
			return this.navigationPage.PushAsync (page);
		}

		public Task<Page> PopPageAsync ()
		{
			return this.navigationPage.PopAsync ();
		}

		public Task PopToRootAsync (bool animate = true)
		{
			return this.navigationPage.PopToRootAsync (animate);
		}

		#endregion

		#region Lifecycle

		protected internal async void OnStart ()
		{
			App app = (App)Application.Current;
			if (app.IsAuthenticated) {
				ApiManager.Initialize (app.EmailAddress, app.Password);
				App.LoadAppliancesIntoCache ();
			}

			await PushPageAsync (new HomePage ());

			if (!app.IsAuthenticated) {
				await ShowLoginPage ();
			} else { // FIXIT: Remove it later when Dashboard is created
				await PushPageAsync (new MyLennarHomePage ());
			}
		}

		protected internal void HandleOrientationChanged ()
		{
			DetailPage detailPage = this.navigationPage.CurrentPage as DetailPage;
			if (detailPage != null) {
				detailPage.OnOrientationChanged ();
			}
		}

		#endregion

		public void AdjustLayout (Page page)
		{
			IsGestureEnabled = !(page is LoginPage);
			if (page is LoginPage) {
				NavigationPage.SetHasNavigationBar (page, false);
			} else {
				var detailPage = page as DetailPage;
				if ((detailPage != null) && detailPage.NoBackButton) {
					NavigationPage.SetHasBackButton (detailPage, false);
				}
			}
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

			App app = (App)Application.Current;
			app.OnSideMenuItemChanged (sideMenuItem);

			this.sideMenuPage.ForceLayout ();
		}

		public async Task LogOut ()
		{
			IsPresented = false;

			await ApiManager.LogOutAsync ();
			await PopToRootAsync (false);
			await ShowLoginPage ();
		}

		public bool IsDocumentPage ()
		{
			return this.navigationPage.CurrentPage is DocumentPage;
		}

		private bool IsSameAsCurrentPage (Page page)
		{
			return (this.navigationPage.CurrentPage != null) && (this.navigationPage.CurrentPage.GetType () == page.GetType ());
		}

		private async Task ShowLoginPage ()
		{
			await PushPageAsync (new LoginPage ());
		}
	}
}

