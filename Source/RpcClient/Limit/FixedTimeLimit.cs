using System.Threading;

using RpcClient.Collect;
using RpcClient.Interface;

using RpcModel;

using SocketTcpServer.Interface;

using RpcHelper;
namespace RpcClient.Limit
{
        internal class FixedTimeLimit : IMsgLimit
        {
                private int _CurrentTime = 0;

                private readonly short _LimitTime = 0;
                private readonly int _LimitNum = 0;

                private int _SurplusNum = 0;

                public bool IsUsable => Interlocked.CompareExchange(ref this._SurplusNum, 0, 0) > 0;

                public FixedTimeLimit(int limitNum, short time)
                {
                        this._LimitTime = time;
                        this._SurplusNum = limitNum;
                        this._LimitNum = limitNum;
                        this._CurrentTime = HeartbeatTimeHelper.HeartbeatTime + time;
                }
                public bool IsLimit()
                {
                        return Interlocked.Decrement(ref this._SurplusNum) < 0;
                }
                public TcpRemoteReply MsgEvent(string key, TcpRemoteMsg msg, ISocketClient client)
                {
                        if (this.IsLimit())
                        {
                                return new TcpRemoteReply(new BasicRes("rpc.exceed.limt"));
                        }
                        return RpcMsgCollect.MsgEvent(new Model.RemoteMsg(key, msg));
                }
                public void Refresh(int time)
                {
                        if (this._CurrentTime <= time)
                        {
                                Interlocked.Exchange(ref this._SurplusNum, this._LimitNum);
                                this._CurrentTime = time + this._LimitTime;
                        }
                }

                public void Reset()
                {
                        Interlocked.Exchange(ref this._SurplusNum, this._LimitNum);
                        this._CurrentTime = HeartbeatTimeHelper.HeartbeatTime + this._LimitTime;
                }
        }
}
