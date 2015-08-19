using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ArcTouchPark
{
	public class GroupedAbdicationList : ObservableCollection<Abdication>
	{
		public GroupedAbdicationList (string groupName, IEnumerable<Abdication> items)
		{
			GroupName = groupName;
			foreach (var item in items) {
				Items.Add (item);
			}
		}

		public string GroupName {
			get;
			set;
		}
	}
}

