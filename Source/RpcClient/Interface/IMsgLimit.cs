using RpcModel;

using SocketTcpServer.Interface;

namespace RpcClient.Interface
{
        internal interface IMsgLimit : IServerLimit
        {
                TcpRemoteReply MsgEvent(string key, TcpRemoteMsg msg, ISocketClient client);
        }
}