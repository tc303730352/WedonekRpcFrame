using RpcClient.Model;

using RpcModel;

namespace RpcClient.Interface
{
        internal interface IRemoteEvent
        {
                TcpRemoteReply MsgEvent(RemoteMsg msg);
        }
}