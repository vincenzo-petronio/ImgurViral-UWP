using System;
using System.Collections.Generic;
using System.Text;
using Windows.Networking.Connectivity;

namespace ImgurViralUWP.Utils
{
    class NetworkHelper
    {
        /// <summary>
        /// Verifica se c'è connettività
        /// </summary>
        /// <returns>bool True/False</returns>
        public static bool CheckConnectivity()
        {
            var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            bool isConnected = connectionProfile != null && connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            System.Diagnostics.Debug.WriteLineIf(connectionProfile != null, "[CheckConnectivity]: \t" + connectionProfile.ProfileName + " = " + isConnected);
            return isConnected;
        }
    }
}
