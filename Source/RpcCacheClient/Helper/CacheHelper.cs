using System.Net;

using RpcHelper;

namespace RpcCacheClient.Helper
{
        internal class CacheHelper
        {
                public static IPEndPoint GetServer(string serverIp, int port)
                {
                        if (serverIp.IsNull())
                        {
                                return null;
                        }
                        else if (serverIp.IndexOf(":") == -1)
                        {
                                return new IPEndPoint(IPAddress.Parse(serverIp), port);
                        }
                        else
                        {
                                string[] t = serverIp.Split(':');
                                return new IPEndPoint(IPAddress.Parse(t[0]), int.Parse(t[1]));
                        }
                }
        }
}
