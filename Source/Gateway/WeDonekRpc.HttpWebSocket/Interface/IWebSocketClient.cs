using System;
using System.IO;
using System.Net;

using WeDonekRpc.HttpWebSocket.Model;

namespace WeDonekRpc.HttpWebSocket.Interface
{
        internal interface IWebSocketClient : IClient, IDisposable
        {
                Guid ClientId { get; }
                bool IsAuthorize { get; }
                /// <summary>
                /// 关闭客户端
                /// </summary>
                void CloseClient();
                /// <summary>
                /// 关闭客户端
                /// </summary>
                /// <param name="time"></param>
                void CloseClient(int time);


                /// <summary>
                /// 心跳
                /// </summary>
                int HeartbeatTime { get; }


                /// <summary>
                /// 会话
                /// </summary>
                ISession Session
                {
                        get;
                }


                /// <summary>
                /// 请求信息
                /// </summary>
                RequestBody Request
                {
                        get;
                }

                /// <summary>
                /// 检查授权
                /// </summary>
                /// <param name="head"></param>
                /// <param name="remoteIp"></param>
                /// <returns></returns>
                bool CheckAuthorize(HttpHead head, IPEndPoint remoteIp);
                /// <summary>
                /// 授权完成
                /// </summary>
                void AuthorizeComplate();
                /// <summary>
                /// 连接关闭事件
                /// </summary>
                void ConCloseEvent();

                /// <summary>
                /// 收到的数据包
                /// </summary>
                /// <param name="currentPage"></param>
                void Receive(DataPageInfo currentPage);


                /// <summary>
                /// 发送ping包
                /// </summary>
                /// <returns></returns>
                bool CheckClient();
                /// <summary>
                /// 发送pong包
                /// </summary>
                /// <returns></returns>
                bool SendPong();

                /// <summary>
                /// 初始化Socket连接
                /// </summary>
                void InitSocket();
        }
}
