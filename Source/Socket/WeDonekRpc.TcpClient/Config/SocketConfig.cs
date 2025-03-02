using System;
using System.Collections.Concurrent;
using System.Net;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.TcpClient.Model;
using WeDonekRpc.TcpClient.SystemAllot;
namespace WeDonekRpc.TcpClient.Config
{
    public class SocketConfig
    {
        private static readonly IConfigSection _Config;
        private static readonly int _DefHeartBeat = Tools.GetRandom(60000, 110000);
        private static string _ServerKey;
        static SocketConfig ()
        {
            _Config = LocalConfig.Local.GetSection("socket:Client");
            ServerPort = _Config.GetValue<int>("Port", 1345);
            ServerIp = _Config.GetValue("ServerIp", "127.0.0.1");
            _ServerKey = _Config.GetValue("ServerKey", "6xy3#7a%ad").GetMd5();
            _Config.AddRefreshEvent(_Refresh);
        }

        private static void _Refresh (IConfigSection section, string name)
        {
            IsSyncRead = section.GetValue<bool>("IsSyncRead", true);
            SyncTimeOut = section.GetValue("SyncTimeOut", 12000);
            SpinWait = section.GetValue<int>("SpinWait", 0);
            TimeOut = section.GetValue("TimeOut", 10);
            ClientNum = section.GetValue<int>("ClientNum", 5);
            UpFileMD5LimitSize = section.GetValue<int>("UpFileMD5LimitSize", 5 * 1024 * 1024);
            HeartbeatTime = section.GetValue<int>("Heartbeat", _DefHeartBeat);
            ConTimeout = section.GetValue("ConTimeout", 5);
            CompressionSize = section.GetValue<ushort>("CompressionSize", 10240);
            IsCompression = section.GetValue<bool>("IsCompression", true);
            UpFileSendTimeOut = section.GetValue<int>("UpFileSendTimeOut", 10);
            ReceiveBufferSize = section.GetValue<int>("ReceiveBufferSize", 32768);
            SendBufferSize = section.GetValue<int>("SendBufferSize", 32768);
            MinBufferSize = section.GetValue<int>("MinBufferSize", 8192);
        }
        /// <summary>
        /// 接收缓冲区大小
        /// </summary>
        public static int ReceiveBufferSize { get; private set; } = 8192;

        /// <summary>
        /// 接收缓冲区大小
        /// </summary>
        public static int MinBufferSize { get; private set; } = 8192;
        /// <summary>
        /// 发送缓冲区大小
        /// </summary>
        public static int SendBufferSize { get; private set; } = 8192;
        private static Interface.ISocketEvent _SocketEvent = null;


        public static Interface.ISocketEvent SocketEvent
        {
            get => SocketConfig._SocketEvent;
            set => SocketConfig._SocketEvent = value;
        }

        private static Interface.IAllot _Allot = new ReplyAllot();

        /// <summary>
        /// 用于接收服务端的广播
        /// </summary>
        public static Interface.IAllot Allot
        {
            get => (Interface.IAllot)SocketConfig._Allot.Clone();
            set => SocketConfig._Allot = value;
        }
        /// <summary>
        /// 服务器IP
        /// </summary>
        public static string ServerIp
        {
            get;
            set;
        } = "127.0.0.1";
        /// <summary>
        /// 压缩最小大小
        /// </summary>
        public static int CompressionSize
        {
            get;
            private set;
        } = 10240;

        /// <summary>
        /// 服务器端口
        /// </summary>
        public static int ServerPort
        {
            get;
            set;
        } = 1345;
        /// <summary>
        /// 是否启用压缩
        /// </summary>
        public static bool IsCompression
        {
            get;
            private set;
        } = true;
        /// <summary>
        /// 服务端密钥
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
        /// 同步等待时间(毫秒)
        /// </summary>
        public static int SpinWait
        {
            get;
            private set;
        } = 0;

        /// <summary>
        /// 心跳间隔时间
        /// </summary>
        public static int HeartbeatTime
        {
            get;
            private set;
        } = 60000;
        /// <summary>
        /// 发送超时时间(秒)
        /// </summary>
        public static int TimeOut
        {
            get;
            private set;
        } = 10;
        /// <summary>
        /// 连接超时时间(秒)
        /// </summary>
        public static int ConTimeout
        {
            get;
            private set;
        } = 7;
        /// <summary>
        /// 是否同步读
        /// </summary>
        public static bool IsSyncRead { get; private set; }
        public static int SyncTimeOut
        {
            get;
            private set;
        } = 12000;
        /// <summary>
        /// 连接池最小链接数
        /// </summary>
        public static int ClientNum { get; private set; } = 5;
        /// <summary>
        /// 上传文件计算MD5最大大小
        /// </summary>
        public static int UpFileMD5LimitSize { get; private set; } = 5242880;
        /// <summary>
        /// 文件上传发送超时时间
        /// </summary>
        public static int UpFileSendTimeOut { get; private set; } = 10;



        private static readonly ConcurrentDictionary<string, ServerConConfig> _ServerConnectArg = new ConcurrentDictionary<string, ServerConConfig>();

        public void SetConArg (string ip, int port, string publicKey, params string[] arg)
        {
            AddConArg(ip, port, publicKey, arg);
        }
        public void SetConArg (string ip, int port, params string[] arg)
        {
            AddConArg(ip, port, arg);
        }
        public static void AddConArg (string ip, int port, params string[] arg)
        {
            AddConArg(ip, port, null, arg);
        }
        public static void AddConArg (Uri uri, string publicKey, params string[] arg)
        {
            string str = uri.DnsSafeHost;
            IPAddress[] address = Dns.GetHostAddresses(uri.DnsSafeHost);
            Array.ForEach(address, a =>
            {
                AddConArg(a.ToString(), uri.Port, publicKey, arg);
            });
        }
        public static void AddConArg (Uri uri, params string[] arg)
        {
            string str = uri.DnsSafeHost;
            IPAddress[] address = Dns.GetHostAddresses(uri.DnsSafeHost);
            Array.ForEach(address, a =>
            {
                AddConArg(a.ToString(), uri.Port, arg);
            });
        }
        public static void AddConArg (string ip, int port, string publicKey, params string[] arg)
        {
            if (arg == null)
            {
                arg = Array.Empty<string>();
            }
            string key = string.Format("{0}:{1}", ip, port);
            publicKey = publicKey == null ? _ServerKey : Tools.GetMD5(publicKey);
            if (_ServerConnectArg.TryGetValue(key, out ServerConConfig config))
            {
                config.Arg = arg;
                config.ConKey = publicKey;
            }
            else
            {
                _ = _ServerConnectArg.TryAdd(key, new ServerConConfig(publicKey, arg));
            }
        }
        internal static ServerConConfig GetConArg (string serverId)
        {
            return _ServerConnectArg.TryGetValue(serverId, out ServerConConfig config) ? config : new ServerConConfig(_ServerKey, null);
        }
    }
}
