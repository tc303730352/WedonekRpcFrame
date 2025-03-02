using System;
using System.Collections.Concurrent;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Limit
{
    /// <summary>
    /// 漏斗-指令限流
    /// </summary>
    internal class LeakLimit : IMsgLimit
    {
        private readonly ConcurrentQueue<LeakRemoteMsg> _MsgQueue = new ConcurrentQueue<LeakRemoteMsg>();

        private readonly int _MaxNum = 0;
        private readonly short _OutNum = 0;
        private readonly Action<LeakRemoteMsg> _Send = null;
        public LeakLimit (int size, short outNum, Action<LeakRemoteMsg> send, ILimitServer server)
        {
            this._Send = send;
            this._OutNum = outNum;
            this._MaxNum = size;
            server.AddEntrust(this._SendMsg);
        }
        public bool IsInvalid => false;
        public bool IsUsable => this._MsgQueue.Count < this._MaxNum;
        public bool IsLimit ()
        {
            return this._MsgQueue.Count > this._MaxNum;
        }
        public TcpRemoteReply MsgEvent (string key, TcpRemoteMsg msg, IIOClient client)
        {
            if (this.IsLimit())
            {
                return new TcpRemoteReply(new BasicRes("rpc.exceed.limt"));
            }
            this._MsgQueue.Enqueue(new LeakRemoteMsg
            {
                Msg = new RemoteMsg(key, msg),
                Client = client,
                ExpireTime = HeartbeatTimeHelper.HeartbeatTime + msg.ExpireTime
            });
            return null;
        }
        private void _SendMsg ()
        {
            int now = HeartbeatTimeHelper.HeartbeatTime;
            for (int i = 0; i < this._OutNum; i++)
            {
                if (!this._MsgQueue.TryDequeue(out LeakRemoteMsg msg))
                {
                    break;
                }
                else if (msg.ExpireTime > now)
                {
                    this._Send(msg);
                }
            }
        }
        public void Refresh (int time)
        {

        }

        public void Reset ()
        {

        }
    }
}
