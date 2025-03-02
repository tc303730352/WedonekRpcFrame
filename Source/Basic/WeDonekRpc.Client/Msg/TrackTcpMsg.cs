using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Client.Track.Model;

using WeDonekRpc.Model;

using WeDonekRpc.TcpServer.Interface;

namespace WeDonekRpc.Client.Msg
{
    internal class TrackTcpMsg : IAllot
    {
        private static readonly TcpRemoteReply _ErrorReply = new TcpRemoteReply(new BasicRes("rpc.msg.page.error"));
        public override object Action ()
        {
            if (RpcClient.IsClosing)
            {
                return new TcpRemoteReply(new BasicRes("rpc.service.close"));
            }
            TcpRemoteMsg msg = this.GetData<TcpRemoteMsg>();
            if (msg == null)
            {
                return _ErrorReply;
            }
            TcpRemoteReply reply = this._Exec(ref msg);
            RpcLogSystem.AddMsgLog(this.Type, msg, reply);
            return reply;
        }
        private TcpRemoteReply _Exec (ref TcpRemoteMsg msg)
        {
            if (msg.Track != null)
            {
                using (TrackBody track = MsgTrackCollect.CreateAnswerTrack(this.Type, msg, this.ClientIp))
                {
                    TcpRemoteReply reply = ServerLimitCollect.MsgEvent(this.Type, msg, this.Client);
                    MsgTrackCollect.EndTrack(track, reply);
                    return reply;
                }
            }
            return ServerLimitCollect.MsgEvent(this.Type, msg, this.Client);
        }
    }
}
