using SocketTcpServer.FileUp.Model;

using RpcHelper;
using RpcHelper.Config;

namespace SocketTcpServer.Config
{
        public class SocketConfig
        {
                private static readonly int _SyncTimeOut = LocalConfig.Local.GetValue("socket_server:SyncTimeOut", 12000);
                private static int _DefaultServerPort = LocalConfig.Local.GetValue("socket_server:Port", 1345);

                /// <summary>
                /// 默认服务端口
                /// </summary>
                public static int DefaultServerPort
                {
                        get => SocketConfig._DefaultServerPort;
                        set => SocketConfig._DefaultServerPort = value;
                }
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

                private static bool _IsCompression = true;
                /// <summary>
                /// 是否启用压缩
                /// </summary>
                public static bool IsCompression
                {
                        get => _IsCompression;
                        set => _IsCompression = value;
                }

                private static ushort _CompressionSize = LocalConfig.Local.GetValue<ushort>("socket_server:CompressionSize", 1000);

                /// <summary>
                /// 压缩最小大小
                /// </summary>
                public static ushort CompressionSize
                {
                        get => SocketConfig._CompressionSize;
                        set => SocketConfig._CompressionSize = value;
                }



                private static string _ServerKey = LocalConfig.Local.GetValue("socket_server:ServerKey", "6xy3#7a%ad").GetMd5();

                /// <summary>
                /// 服务器的链接密钥
                /// </summary>
                public static string ServerKey
                {
                        get => SocketConfig._ServerKey;
                        set
                        {
                                if (string.IsNullOrEmpty(value))
                                {
                                        return;
                                }
                                SocketConfig._ServerKey = Tools.GetMD5(value);
                        }
                }


                private static readonly string _LocalIp = LocalConfig.Local.GetValue("socket_server:LocalIp", "127.0.0.1");

                /// <summary>
                /// 当前IP地址
                /// </summary>
                public static string LocalIp => SocketConfig._LocalIp;

                /// <summary>
                /// 同步超时时间
                /// </summary>
                public static int SyncTimeOut => _SyncTimeOut;
                /// <summary>
                /// 超时时间
                /// </summary>
                public static int TimeOut { get; private set; } = LocalConfig.Local.GetValue("socket_server:TimeOut", 10);

                /// <summary>
                /// 块大小(5MB)
                /// </summary>
                public static FileUpConfig UpConfig
                {
                        get;
                        set;
                } = LocalConfig.Local.GetValue("socket_server:UpConfig", new FileUpConfig { BlockSize = 65, UpTimeOut = 60 });
        }
}
