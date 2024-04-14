using System.Net.NetworkInformation;

namespace PopupCash.Account.Models.Helpers
{
    public class MacAddressHelper
    {
        public static string GetMacAddress()
        {
            string macAddress = string.Empty;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddress = nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddress;
        }
    }
}
