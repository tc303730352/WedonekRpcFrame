using RpcClient.Collect;

using RpcModel;

using SocketTcpServer.Interface;
namespace RpcClient.Msg
{
        internal class TcpMsg : IAllot
        {
                public override object Action()
                {
                        if (RpcClient.IsClosing)
                        {
                                return new TcpRemoteReply(new BasicRes("rpc.service.close"));
                        }
                        TcpRemoteMsg msg = this.GetData<TcpRemoteMsg>();
                        if (msg != null)
                        {
                                TcpRemoteReply reply = ServerLimitCollect.MsgEvent(this.Type, msg, this.Clinet);
                                RpcLogSystem.AddMsgLog(this.Type, msg, reply);
                                return reply;
                        }
                        return new TcpRemoteReply(new BasicRes("rpc.msg.page.error"));
                }
        }
}
