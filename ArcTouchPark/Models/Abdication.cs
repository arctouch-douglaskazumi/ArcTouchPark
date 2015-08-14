using System;


namespace ArcTouchPark
{
	public class Abdication : Parseable
	{
		public Abdication ()
		{
		}

		public string Username {
			get;
			set;
		}

		public DateTime SelectedDate {
			get;
			set;
		}

		public string ReplacedByUsername {
			get;
			set;
		}
	}
}

