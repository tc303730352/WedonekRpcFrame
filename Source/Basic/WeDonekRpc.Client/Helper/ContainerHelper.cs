using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Helper
{
    public class ContainerHelper
    {
        /// <summary>
        /// 获取当前运行程序的容器Id
        /// </summary>
        /// <returns></returns>
        public static string ContainerId ()
        {
            string hostname = Environment.GetEnvironmentVariable("HOSTNAME", EnvironmentVariableTarget.Process);
            return string.IsNullOrWhiteSpace(hostname) ? string.Empty : hostname;
        }
        public static string GetLoaclIp ()
        {
            NetworkInterface[] networks = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface network in networks)
            {
                IPInterfaceProperties ip = network.GetIPProperties();
                if (ip != null && ip.UnicastAddresses.Count > 0)
                {
                    UnicastIPAddressInformation info = ip.UnicastAddresses.FirstOrDefault(p => p.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !IPAddress.IsLoopback(p.Address));
                    if (info != null)
                    {
                        return info.Address.ToString();
                    }
                }
            }
            return null;
        }
        public static int? GetInsidePort (int? def)
        {
            string dir = Environment.GetEnvironmentVariable("PWD", EnvironmentVariableTarget.Process);
            if (dir.IsNull())
            {
                dir = AppDomain.CurrentDomain.BaseDirectory;
            }
            string path = Path.Combine(dir, "Dockerfile");
            if (!File.Exists(path))
            {
                return def;
            }
            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                do
                {
                    string line = sr.ReadLine();
                    if (line == null)
                    {
                        return def;
                    }
                    line = line.Trim();
                    if (line.StartsWith("EXPOSE "))
                    {
                        string[] ip = line.Remove(0, 7).TrimStart().Split(' ');
                        if (ip.IsNull())
                        {
                            return def;
                        }
                        string t = ip[0];
                        if (t.IndexOf('/') == -1)
                        {
                            return int.Parse(t);
                        }
                        else
                        {
                            return int.Parse(t[..t.IndexOf('/')]);
                        }
                    }
                } while (true);
            }
        }
        internal static string GetPhysicalIp ()
        {
            IPAddress[] addrs = Dns.GetHostAddresses("host.docker.internal");
            if (addrs.IsNull())
            {
                return string.Empty;
            }
            return addrs[0].ToString();
        }
    }
}
