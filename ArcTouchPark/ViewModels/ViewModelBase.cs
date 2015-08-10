using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ArcTouchPark
{
	public abstract class ViewModelBase : INotifyPropertyChanged
	{
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

