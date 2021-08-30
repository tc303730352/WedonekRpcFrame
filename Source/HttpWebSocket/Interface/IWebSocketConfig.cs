using System;
using System.Security.Cryptography.X509Certificates;

namespace HttpWebSocket.Interface
{
        /// <summary>
        /// 配置项
        /// </summary>
        public interface IWebSocketConfig
        {
                /// <summary>
                /// 发送数据流的缓存区大小
                /// </summary>
                int BufferSize { get; set; }
                /// <summary>
                /// 证书
                /// </summary>
                X509Certificate2 Certificate { get; set; }
                /// <summary>
                /// 心跳时间（秒）
                /// </summary>
                int HeartbeatTime { get; set; }
                /// <summary>
                /// 固定值
                /// </summary>
                string ResponseGuid { get; }
                /// <summary>
                /// 服务端口
                /// </summary>
                int ServerPort { get; set; }

                /// <summary>
                /// Socket事件
                /// </summary>
                ISocketEvent SocketEvent { get; }
                /// <summary>
                /// 连接队列长度(默认5000)
                /// </summary>
                int TcpBacklog { get; set; }
                /// <summary>
                /// 是否单点(根据授权ID)
                /// </summary>
                bool IsSingle { get; set; }
                /// <summary>
                /// 获取链接完整URL
                /// </summary>
                /// <returns></returns>
                Uri ToUri();
        }
}