using System;

namespace ArcTouchPark
{

	public interface ILogging
	{

		void Log (string message);

		void LogError (string error, Exception exception = null);
	}
}

