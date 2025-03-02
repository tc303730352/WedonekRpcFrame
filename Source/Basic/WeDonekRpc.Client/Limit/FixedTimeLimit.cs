using System.Threading;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Limit
{
    /// <summary>
    /// 固定时间窗-指令限流
    /// </summary>
    internal class FixedTimeLimit : IMsgLimit
    {
        private int _CurrentTime = 0;

        private readonly short _LimitTime = 0;
        private readonly int _LimitNum = 0;

        private int _SurplusNum = 0;

        public bool IsUsable => Interlocked.CompareExchange(ref this._SurplusNum, 0, 0) > 0;

        public bool IsInvalid => false;

        public FixedTimeLimit (int limitNum, short time)
        {
            this._LimitTime = time;
            this._SurplusNum = limitNum;
            this._LimitNum = limitNum;
            this._CurrentTime = HeartbeatTimeHelper.HeartbeatTime + time;
        }
        public bool IsLimit ()
        {
            return Interlocked.Decrement(ref this._SurplusNum) < 0;
        }
        public TcpRemoteReply MsgEvent (string key, TcpRemoteMsg msg, IIOClient client)
        {
            if (this.IsLimit())
            {
                return new TcpRemoteReply(new BasicRes("rpc.exceed.limt"));
            }
            return RpcMsgCollect.MsgEvent(new Model.RemoteMsg(key, msg));
        }
        public void Refresh (int time)
        {
            if (this._CurrentTime <= time)
            {
                _ = Interlocked.Exchange(ref this._SurplusNum, this._LimitNum);
                this._CurrentTime = time + this._LimitTime;
            }
        }

        public void Reset ()
        {
            _ = Interlocked.Exchange(ref this._SurplusNum, this._LimitNum);
            this._CurrentTime = HeartbeatTimeHelper.HeartbeatTime + this._LimitTime;
        }
    }
}
