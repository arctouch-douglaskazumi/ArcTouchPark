using System;

using ArcTouchPark;

[assembly: Xamarin.Forms.Dependency (typeof(ArcTouchPark.Droid.Logging))]
namespace ArcTouchPark.Droid
{

	public class Logging : ILogging
	{

		public Logging ()
		{
		}

		#region ILogging implementation

		public void Log (string message)
		{
			Console.WriteLine (message);
		}

		public void LogError (string error, Exception exception = null)
		{
			Console.WriteLine ("********************************");
			Console.WriteLine ("Error: " + error);
			if (exception != null) {
				Console.WriteLine (exception.StackTrace);
				Xamarin.Insights.Report (exception);
			}
		}

		#endregion
	}
}

