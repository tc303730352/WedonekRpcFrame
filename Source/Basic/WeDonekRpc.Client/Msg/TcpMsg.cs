using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Model;

using WeDonekRpc.TcpServer.Interface;
namespace WeDonekRpc.Client.Msg
{
    internal class TcpMsg : IAllot
    {
        public override object Action ()
        {
            if (RpcClient.IsClosing)
            {
                return new TcpRemoteReply(new BasicRes("rpc.service.close"));
            }
            TcpRemoteMsg msg = this.GetData<TcpRemoteMsg>();
            if (msg != null)
            {
                TcpRemoteReply reply = ServerLimitCollect.MsgEvent(this.Type, msg, this.Client);
                RpcLogSystem.AddMsgLog(this.Type, msg, reply);
                return reply;
            }
            return new TcpRemoteReply(new BasicRes("rpc.msg.page.error"));
        }
    }
}
