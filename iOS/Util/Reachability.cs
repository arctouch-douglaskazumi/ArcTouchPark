﻿using System;
using System.Net;
using SystemConfiguration;

using CoreFoundation;

namespace ArcTouchPark.iOS
{
	public enum NetworkStatus
	{
		NotReachable,
		ReachableViaCarrierDataNetwork,
		ReachableViaWiFiNetwork
	}

	/// <summary>
	/// Reachability.
	/// Link: http://developer.xamarin.com/samples/monotouch/ReachabilitySample/
	/// </summary>
	public static class Reachability
	{
		public static string HostName = "www.google.com";

		public static bool IsReachableWithoutRequiringConnection (NetworkReachabilityFlags flags)
		{
			bool isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;

			bool noConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0;

			if ((flags & NetworkReachabilityFlags.IsWWAN) != 0) {
				noConnectionRequired = true;
			}

			return isReachable && noConnectionRequired;
		}

		public static bool IsHostReachable (string host)
		{
			if (string.IsNullOrEmpty (host)) {
				return false;
			}

			using (var r = new NetworkReachability (host)) {
				NetworkReachabilityFlags flags;

				if (r.TryGetFlags (out flags)) {
					return IsReachableWithoutRequiringConnection (flags);
				}
			}
			return false;
		}

		public static event EventHandler ReachabilityChanged;

		public static void OnChange (NetworkReachabilityFlags flags)
		{
			var h = ReachabilityChanged;
			if (h != null) {
				h (null, EventArgs.Empty);
			}
		}

		public static NetworkReachability adHocWiFiNetworkReachability;

		public static bool IsAdHocWiFiNetworkAvailable (out NetworkReachabilityFlags flags)
		{
			if (adHocWiFiNetworkReachability == null) {
				adHocWiFiNetworkReachability = new NetworkReachability (new IPAddress (new byte [] { 169, 254, 0, 0 }));
				adHocWiFiNetworkReachability.SetNotification (OnChange);
				adHocWiFiNetworkReachability.Schedule (CFRunLoop.Current, CFRunLoop.ModeDefault);
			}

			return adHocWiFiNetworkReachability.TryGetFlags (out flags) && IsReachableWithoutRequiringConnection (flags);
		}

		public static NetworkReachability defaultRouteReachability;

		public static bool IsNetworkAvailable (out NetworkReachabilityFlags flags)
		{
			if (defaultRouteReachability == null) {
				defaultRouteReachability = new NetworkReachability (new IPAddress (0));
				defaultRouteReachability.SetNotification (OnChange);
				defaultRouteReachability.Schedule (CFRunLoop.Current, CFRunLoop.ModeDefault);
			}
			return defaultRouteReachability.TryGetFlags (out flags) && IsReachableWithoutRequiringConnection (flags);
		}

		public static NetworkReachability remoteHostReachability;

		public static NetworkStatus RemoteHostStatus ()
		{
			NetworkReachabilityFlags flags;
			bool reachable;

			if (remoteHostReachability == null) {
				remoteHostReachability = new NetworkReachability (HostName);

				reachable = remoteHostReachability.TryGetFlags (out flags);

				remoteHostReachability.SetNotification (OnChange);
				remoteHostReachability.Schedule (CFRunLoop.Current, CFRunLoop.ModeDefault);
			} else {
				reachable = remoteHostReachability.TryGetFlags (out flags);  
			}

			if (!reachable) {
				return NetworkStatus.NotReachable;
			}

			if (!IsReachableWithoutRequiringConnection (flags)) {
				return NetworkStatus.NotReachable;
			}

			if ((flags & NetworkReachabilityFlags.IsWWAN) != 0) {
				return NetworkStatus.ReachableViaCarrierDataNetwork;
			}

			return NetworkStatus.ReachableViaWiFiNetwork;
		}

		public static NetworkStatus InternetConnectionStatus ()
		{
			NetworkReachabilityFlags flags;
			bool defaultNetworkAvailable = IsNetworkAvailable (out flags);
			if (defaultNetworkAvailable && ((flags & NetworkReachabilityFlags.IsDirect) != 0)) {
				return NetworkStatus.NotReachable;
			} else if ((flags & NetworkReachabilityFlags.IsWWAN) != 0) {
				return NetworkStatus.ReachableViaCarrierDataNetwork;
			} else if (flags == 0) {
				return NetworkStatus.NotReachable;
			}
			return NetworkStatus.ReachableViaWiFiNetwork;
		}

		public static NetworkStatus LocalWifiConnectionStatus ()
		{
			NetworkReachabilityFlags flags;
			if (IsAdHocWiFiNetworkAvailable (out flags)) {
				if ((flags & NetworkReachabilityFlags.IsDirect) != 0) {
					return NetworkStatus.ReachableViaWiFiNetwork;
				}
			}
			return NetworkStatus.NotReachable;
		}
	}
}



