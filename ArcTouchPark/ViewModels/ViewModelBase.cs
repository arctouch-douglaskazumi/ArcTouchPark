using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ArcTouchPark
{
	public abstract class ViewModelBase : INotifyPropertyChanged
	{
		private bool isRunning;

		public bool IsRunning {
			get {
				return this.isRunning;
			}

			set {
				if (this.isRunning != value) {
					this.isRunning = value;
					RaisePropertyChanged ();
				}
			}
		}

		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		protected internal virtual void RaisePropertyChanged ([CallerMemberName] string fieldName = "")
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) {
				handler (this, new PropertyChangedEventArgs (fieldName));
			} 
		}

		#endregion
	}
}

