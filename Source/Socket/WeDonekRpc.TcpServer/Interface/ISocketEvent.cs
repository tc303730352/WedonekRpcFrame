using System;
using System.Net;

namespace WeDonekRpc.TcpServer.Interface
{
        public interface ISocketEvent
        {
                /// <summary>
                /// 客户端建立连接
                /// </summary>
                /// <param name="clientId">客户端ID</param>
                /// <param name="ipAddress">客户端IP地址</param>
                /// <param name="arg">附带参数</param>
                /// <param name="bindParam">客户端绑定参数</param>
                /// <param name="error">错误信息</param>
                /// <returns>是否允许建立连接</returns>
                bool ClientBuildConnect(Guid clientId, IPEndPoint ipAddress, string[] arg, out string bindParam, out string error);

                /// <summary>
                /// 客户端连接关闭
                /// </summary>
                /// <param name="clientId">客户端ID</param>
                /// <param name="bindParam">绑定参数</param>
                void ClientConnectClose(Guid clientId, string bindParam);
        }
}
