using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ArcTouchPark
{
	public partial class MainPage : ContentPage
	{
		private MainPageViewModel mainPageViewModel;

		public MainPage ()
		{
			this.mainPageViewModel = new MainPageViewModel ();
			BindingContext = this.mainPageViewModel;
			InitializeComponent ();
		}
	}
}

