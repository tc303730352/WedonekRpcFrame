
namespace WeDonekRpc.TcpClient.Interface
{
        public interface ISocketEvent
        {
                /// <summary>
                /// 服务端连接错误
                /// </summary>
                /// <param name="errorInfo"></param>
                void ServerConnectError(string errorInfo);

                /// <summary>
                /// 服务器连接关闭
                /// </summary>
                void ServerConClose();
        }
}
