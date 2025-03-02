using System.Text;

namespace WeDonekRpc.HttpWebSocket.Config
{
    public class PublicConfig
    {
        /// <summary>
        /// 会话最大离线时间
        /// </summary>
        public static int OfflineLimit = 2;
        /// <summary>
        /// 最大请求头大小
        /// </summary>
        public static int MaxHeadSize = 5000;

        /// <summary>
        /// 请求编码
        /// </summary>
        public static Encoding RequestEncoding = Encoding.UTF8;
        /// <summary>
        /// 响应编码
        /// </summary>
        public static Encoding ResponseEncoding = Encoding.UTF8;
    }
}
