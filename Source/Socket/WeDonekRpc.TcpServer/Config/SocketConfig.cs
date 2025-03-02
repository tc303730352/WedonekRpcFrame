using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.TcpServer.FileUp.Model;

namespace WeDonekRpc.TcpServer.Config
{
    public class SocketConfig
    {
        private static readonly IConfigSection _Config;

        private static string _ServerKey;
        static SocketConfig ()
        {
            _Config = LocalConfig.Local.GetSection("socket:Service");
            DefaultServerPort = _Config.GetValue("Port", 1345);
            _ServerKey = _Config.GetValue("ServerKey", "6xy3#7a%ad").GetMd5();
            _Config.AddRefreshEvent(_Refresh);
        }

        private static void _Refresh (IConfigSection section, string name)
        {
            IsSyncRead = section.GetValue<bool>("IsSyncRead", true);
            UpConfig = section.GetValue("UpConfig", new FileUpConfig());
            ReplyRetryNum = section.GetValue("ReplyRetryNum", 3);
            CompressionSize = section.GetValue<ushort>("CompressionSize", 10240);
            IsCompression = section.GetValue<bool>("IsCompression", true);
            ReceiveBufferSize = section.GetValue<int>("ReceiveBufferSize", 32768);
            SendBufferSize = section.GetValue<int>("SendBufferSize", 32768);
            MinBufferSize = section.GetValue<int>("MinBufferSize", 8192);
        }
        /// <summary>
        /// 接收缓冲区大小
        /// </summary>
        public static int MinBufferSize { get; private set; } = 8192;
        /// <summary>
        /// 接收缓冲区大小
        /// </summary>
        public static int ReceiveBufferSize { get; private set; } = 32768;

        /// <summary>
        /// 发送缓冲区大小
        /// </summary>
        public static int SendBufferSize { get; private set; } = 32768;
        /// <summary>
        /// 是否同步读
        /// </summary>
        public static bool IsSyncRead { get; private set; }
        /// <summary>
        /// 默认服务端口
        /// </summary>
        public static int DefaultServerPort
        {
            get;
            set;
        } = 1345;
        private static Interface.IAllot _DefaultAllot = null;

        /// <summary>
        /// 默认的处理程序
        /// </summary>
        public static Interface.IAllot DefaultAllot
        {
            get => SocketConfig._DefaultAllot;
            set => SocketConfig._DefaultAllot = value;
        }

        private static Interface.ISocketEvent _SocketEvent = null;

        /// <summary>
        /// socket事件
        /// </summary>
        public static Interface.ISocketEvent SocketEvent
        {
            get => SocketConfig._SocketEvent;
            set => SocketConfig._SocketEvent = value;
        }

        /// <summary>
        /// 是否启用压缩
        /// </summary>
        public static bool IsCompression
        {
            get;
            private set;
        }
        /// <summary>
        /// 压缩最小大小
        /// </summary>
        public static ushort CompressionSize
        {
            get;
            private set;
        }
        /// <summary>
        /// 服务器的链接密钥
        /// </summary>
        public static string ServerKey
        {
            get => _ServerKey;
            set
            {
                if (value.IsNotNull())
                {
                    _ServerKey = value.GetMd5();
                }
            }
        }
        /// <summary>
        /// 回复重试数
        /// </summary>
        public static int ReplyRetryNum { get; private set; } = 3;

        /// <summary>
        /// 块大小(5MB)
        /// </summary>
        public static FileUpConfig UpConfig
        {
            get;
            private set;
        } = new FileUpConfig();
    }
}
