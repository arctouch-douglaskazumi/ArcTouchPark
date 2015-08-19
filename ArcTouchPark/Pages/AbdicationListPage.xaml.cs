using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ArcTouchPark
{
	public partial class AbdicationListPage : ContentPage
	{
		private AbdicationListPageViewModel abdicationListPageViewModel;

		public AbdicationListPage ()
		{
			abdicationListPageViewModel = new AbdicationListPageViewModel ();
			BindingContext = abdicationListPageViewModel;
			InitializeComponent ();
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			AbdicationListView.ItemTapped += abdicationListPageViewModel.GetSpot;
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();
			AbdicationListView.ItemTapped -= abdicationListPageViewModel.GetSpot;
		}
	}
}

