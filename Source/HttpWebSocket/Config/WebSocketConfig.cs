using System;
using System.Security.Cryptography.X509Certificates;

using HttpWebSocket.Interface;

namespace HttpWebSocket.Config
{
        public class WebSocketConfig : IWebSocketConfig
        {

                public WebSocketConfig() : this(new SocketEvent())
                {

                }
                public WebSocketConfig(ISocketEvent socketEvent)
                {
                        this.SocketEvent = socketEvent;
                }

                /// <summary>
                /// 心跳时间(秒)
                /// </summary>
                public int HeartbeatTime { get; set; } = 10;

                /// <summary>
                /// 响应的GUID
                /// </summary>
                public string ResponseGuid { get; set; } = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";


                /// <summary>
                /// 缓冲区大小
                /// </summary>
                public int BufferSize { get; set; } = 5242880;

                /// <summary>
                /// 服务端口
                /// </summary>
                public int ServerPort { get; set; } = 1254;

                /// <summary>
                /// HTTPS证书
                /// </summary>
                public X509Certificate2 Certificate { get; set; }


                public ISocketEvent SocketEvent { get; }

                /// <summary>
                /// TCP排队数量
                /// </summary>
                public int TcpBacklog { get; set; } = 5000;
                /// <summary>
                /// 是否单点登陆
                /// </summary>
                public bool IsSingle { get; set; } = false;

                public Uri ToUri()
                {
                        if (this.Certificate == null)
                        {
                                return new Uri(string.Format("ws://127.0.0.1:{0}/", this.ServerPort));
                        }
                        return new Uri(string.Format("wss://127.0.0.1:{0}/", this.ServerPort));
                }
        }
}
