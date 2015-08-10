using System.Threading.Tasks;

namespace ArcTouchPark
{
	public interface IEnvironment
	{
		string PersonalFolder { get; }

		bool IsInternetAvailable { get; }
	}
}