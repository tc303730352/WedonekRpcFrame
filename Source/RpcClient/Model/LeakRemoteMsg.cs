using SocketTcpServer.Interface;

namespace RpcClient.Model
{
        internal class LeakRemoteMsg
        {
                public RemoteMsg Msg { get; set; }
                public ISocketClient Client { get; set; }
                public int ExpireTime { get; set; }
        }
}
