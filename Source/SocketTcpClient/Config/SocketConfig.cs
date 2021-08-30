using System;
using System.Collections.Concurrent;
using System.Net;

using SocketTcpClient.Model;
using SocketTcpClient.SystemAllot;

using RpcHelper;
using RpcHelper.Config;
namespace SocketTcpClient.Config
{
        public class SocketConfig
        {

                private static int _SyncTimeOut = LocalConfig.Local.GetValue("socket:SyncTimeOut", 12000);

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
                private static string _ServerIp = "127.0.0.1";

                /// <summary>
                /// 服务器IP
                /// </summary>
                public static string ServerIp
                {
                        get => SocketConfig._ServerIp;
                        set => SocketConfig._ServerIp = value;
                }

                private static int _CompressionSize = LocalConfig.Local.GetValue<int>("socket:CompressionSize", 10240);

                /// <summary>
                /// 压缩最小大小
                /// </summary>
                public static int CompressionSize
                {
                        get => SocketConfig._CompressionSize;
                        set => SocketConfig._CompressionSize = value;
                }

                /// <summary>
                /// 服务器端口
                /// </summary>
                private static int _ServerPort = LocalConfig.Local.GetValue<ushort>("socket:port", 1345);

                /// <summary>
                /// 服务器端口
                /// </summary>
                public static int ServerPort
                {
                        get => SocketConfig._ServerPort;
                        set => SocketConfig._ServerPort = value;
                }
                private static bool _IsCompression = true;

                /// <summary>
                /// 是否启用压缩
                /// </summary>
                public static bool IsCompression
                {
                        get => SocketConfig._IsCompression;
                        set => SocketConfig._IsCompression = value;
                }

                private static string _ServerKey = LocalConfig.Local.GetValue("socket:ServerKey", "6xy3#7a%ad").GetMd5();

                /// <summary>
                /// 服务端密钥
                /// </summary>
                public static string ServerKey
                {
                        get => SocketConfig._ServerKey;
                        set => SocketConfig._ServerKey = Tools.GetMD5(value);
                }
                /// <summary>
                /// 同步等待时间(毫秒)
                /// </summary>
                public static int SpinWait
                {
                        get;
                } = LocalConfig.Local.GetValue<ushort>("socket:SpinWait", 50);
                private static readonly ConcurrentDictionary<string, ServerConConfig> _ServerConnectArg = new ConcurrentDictionary<string, ServerConConfig>();

                public void SetConArg(string ip, int port, string publicKey, params string[] arg)
                {
                        AddConArg(ip, port, publicKey, arg);
                }
                public void SetConArg(string ip, int port, params string[] arg)
                {
                        AddConArg(ip, port, arg);
                }
                public static void AddConArg(string ip, int port, params string[] arg)
                {
                        AddConArg(ip, port, null, arg);
                }
                public static void AddConArg(Uri uri, string publicKey, params string[] arg)
                {
                        string str = uri.DnsSafeHost;
                        IPAddress[] address = Dns.GetHostAddresses(uri.DnsSafeHost);
                        Array.ForEach(address, a =>
                        {
                                AddConArg(a.ToString(), uri.Port, publicKey, arg);
                        });
                }
                public static void AddConArg(Uri uri, params string[] arg)
                {
                        string str = uri.DnsSafeHost;
                        IPAddress[] address = Dns.GetHostAddresses(uri.DnsSafeHost);
                        Array.ForEach(address, a =>
                        {
                                AddConArg(a.ToString(), uri.Port, arg);
                        });
                }
                public static void AddConArg(string ip, int port, string publicKey, params string[] arg)
                {
                        if (arg == null)
                        {
                                arg = new string[0];
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
                                _ServerConnectArg.TryAdd(key, new ServerConConfig(publicKey, arg));
                        }
                }
                internal static ServerConConfig GetConArg(string serverId)
                {
                        return _ServerConnectArg.TryGetValue(serverId, out ServerConConfig config) ? config : new ServerConConfig(_ServerKey, null);
                }


                private static int _HeartbeatTime = Tools.GetRandom(60000, 110000);

                /// <summary>
                /// 心跳间隔时间
                /// </summary>
                public static int HeartbeatTime
                {
                        get => SocketConfig._HeartbeatTime;
                        set => SocketConfig._HeartbeatTime = value;
                }
                /// <summary>
                /// 发送超时时间(秒)
                /// </summary>
                private static int _Timeout = LocalConfig.Local.GetValue<int>("socket:TimeOut", 10);

                public static int Timeout
                {
                        get => SocketConfig._Timeout;
                        set => SocketConfig._Timeout = value;
                }
                /// <summary>
                /// 连接超时时间(秒)
                /// </summary>
                private static int _ConTimeout = LocalConfig.Local.GetValue<int>("socket:ConTimeout", 5);

                public static int ConTimeout
                {
                        get => SocketConfig._ConTimeout;
                        set => SocketConfig._ConTimeout = value;
                }
                public static int SyncTimeOut
                {
                        get => _SyncTimeOut;

                        set => _SyncTimeOut = value;
                }
                /// <summary>
                /// 连接池最小链接数
                /// </summary>
                public static int ClientNum { get; set; } = LocalConfig.Local.GetValue<int>("socket:ClientNum", 2);
                /// <summary>
                /// 上传文件计算MD5最大大小
                /// </summary>
                public static int UpFileMD5LimitSize { get; set; } = LocalConfig.Local.GetValue<int>("socket:UpFileMD5LimitSize", 5 * (1024 * 1024));
                /// <summary>
                /// 文件上传发送超时时间
                /// </summary>
                public static int UpFileSendTimeOut { get; set; } = LocalConfig.Local.GetValue<int>("socket:UpFileSendTimeOut", 10);
        }
}
