using System.Net;

using WeDonekRpc.Helper;

namespace WeDonekRpc.CacheClient.Helper
{
    /// <summary>
    /// 缓存帮助
    /// </summary>
    internal class CacheHelper
    {
        /// <summary>
        /// 获取服务地址
        /// </summary>
        /// <param name="serverIp">服务IP</param>
        /// <param name="port">端口</param>
        /// <returns>服务地址</returns>
        public static IPEndPoint GetServer(string serverIp, int port)
        {
            if (serverIp.IsNull())
            {
                return null;
            }
            else if (!serverIp.Contains(':'))
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
