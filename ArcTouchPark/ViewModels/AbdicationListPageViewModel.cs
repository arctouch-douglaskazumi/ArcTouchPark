using System;
using System.Windows.Input;
using System.Linq;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ArcTouchPark
{
	public class AbdicationListPageViewModel : ViewModelBase
	{
		public AbdicationListPageViewModel ()
		{
			Initialize ();
		}

		public App App {
			get {
				return (App)Application.Current;
			}
		}

		private ObservableCollection<GroupedAbdicationList> abdicationList;

		public ObservableCollection<GroupedAbdicationList> AbdicationList {
			get {
				return this.abdicationList;
			}

			set {
				if (this.abdicationList != value) {
					this.abdicationList = value;
					RaisePropertyChanged ();
				}
			}
		}

		protected async void Initialize ()
		{
			await RefreshAsync ();
			RefreshCommand = new Command (Refresh);
		}

		public async void Refresh ()
		{
			await RefreshAsync ();
		}

		public ICommand RefreshCommand { get; private set; }

		private async Task RefreshAsync ()
		{
			IsRunning = true;
			try {
				var abList = await ParseApi.GetAllAsync<Abdication> ();
				var grouped = abList
					.OrderByDescending (ab => ab.SelectedDate)
					.GroupBy (ab => ab.SelectedDate.ToString (Const.DATE_FORMAT))
					.Select (g => new GroupedAbdicationList (
					              Strings.ListDateLabel + g.Key,
					              g.Select (groupedAbd => new Abdication {
						objectId = groupedAbd.objectId,
						SelectedDate = groupedAbd.SelectedDate,
						Username = Strings.ListAbdicatorLabel + groupedAbd.Username,
						ReplacedByUsername = Strings.ListReplacedByLabel + groupedAbd.ReplacedByUsername
					})
				              ));
				AbdicationList = new ObservableCollection<GroupedAbdicationList> (grouped);
			} finally {
				IsRunning = false;
			}
		}

		public async void GetSpot (object sender, ItemTappedEventArgs itemTappedEventArgs)
		{
			var abdication = (Abdication)itemTappedEventArgs.Item;
			var replaceByWithoutLabel = abdication.ReplacedByUsername.Replace (Strings.ListReplacedByLabel, string.Empty);
			if (!string.IsNullOrWhiteSpace (replaceByWithoutLabel)) {
				await App.DisplayAlertAsync (Strings.SpotAlreadyTaken);
				return;
			}

			try {
				MainPageViewModel mPVM = new MainPageViewModel ();
				IsRunning = true;
				await mPVM.NotificationClickedAsync (abdication.objectId);
				await RefreshAsync ();
			} finally {
				IsRunning = false;
			}
		}
	}
}

