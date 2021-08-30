using RpcClient.Collect;
using RpcClient.Track.Model;

using RpcModel;

using SocketTcpServer.Interface;

namespace RpcClient.Msg
{
        internal class TrackTcpMsg : IAllot
        {
                private static readonly TcpRemoteReply _ErrorReply = new TcpRemoteReply(new BasicRes("rpc.msg.page.error"));
                public override object Action()
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
                        TcpRemoteReply reply = this._Exec(msg);
                        RpcLogSystem.AddMsgLog(this.Type, msg, reply);
                        return reply;
                }
                private TcpRemoteReply _Exec(TcpRemoteMsg msg)
                {
                        if (msg.Track != null)
                        {
                                using (TrackBody track = MsgTrackCollect.CreateAnswerTrack(this.Type, msg, this.ClientIp))
                                {
                                        TcpRemoteReply reply = ServerLimitCollect.MsgEvent(this.Type, msg, this.Clinet);
                                        MsgTrackCollect.EndTrack(track, reply);
                                        return reply;
                                }
                        }
                        return ServerLimitCollect.MsgEvent(this.Type, msg, this.Clinet);
                }
        }
}
