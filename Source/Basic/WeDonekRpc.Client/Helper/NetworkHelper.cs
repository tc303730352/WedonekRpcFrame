using System.Linq;
using System.Net.NetworkInformation;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Helper
{
    public class NetworkBody
    {
        public string Ip
        {
            get;
            set;
        }
        public string Mac
        {
            get;
            set;
        }
    }
    internal class NetworkHelper
    {
        public static string GetMac ()
        {
            NetworkInterface[] networks = NetworkInterface.GetAllNetworkInterfaces().FindAll(a => a.NetworkInterfaceType != NetworkInterfaceType.Loopback && a.OperationalStatus == OperationalStatus.Up);
            if (networks.Length > 0)
            {
                NetworkInterface network = networks[0];
                return Tools.FormatMac(network.GetPhysicalAddress().ToString());
            }
            return null;
        }
        public static NetworkBody GetNetwork ()
        {
            NetworkInterface[] networks = NetworkInterface.GetAllNetworkInterfaces().FindAll(a => a.NetworkInterfaceType != NetworkInterfaceType.Loopback && a.OperationalStatus == OperationalStatus.Up);
            if (networks.Length > 0)
            {
                NetworkInterface network = networks[0];
                IPInterfaceProperties ip = network.GetIPProperties();
                return new NetworkBody
                {
                    Mac = Tools.FormatMac(network.GetPhysicalAddress().ToString()),
                    Ip = ip.UnicastAddresses.Where(a => a.PrefixOrigin == PrefixOrigin.Dhcp).Select(a => a.Address.ToString()).FirstOrDefault()
                };
            }
            return null;
        }
    }
}
