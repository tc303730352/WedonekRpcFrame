using RpcClient.Collect;
using RpcClient.Interface;

using RpcModel;

using SocketTcpServer.Interface;

namespace RpcClient.Limit
{
        internal class NoEnableLimit : IMsgLimit
        {
                public bool IsUsable => true;

                public bool IsLimit()
                {
                        return false;
                }

                public TcpRemoteReply MsgEvent(string key, TcpRemoteMsg msg, ISocketClient client)
                {
                        return RpcMsgCollect.MsgEvent(new Model.RemoteMsg(key, msg));
                }

                public void Refresh(int time)
                {

                }

                public void Reset()
                {

                }
        }
}
