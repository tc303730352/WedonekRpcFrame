using System;
using System.Collections.Concurrent;

using RpcClient.Interface;
using RpcClient.Model;

using RpcModel;

using SocketTcpServer.Interface;

using RpcHelper;

namespace RpcClient.Limit
{

        internal class LeakLimit : IMsgLimit
        {
                private readonly ConcurrentQueue<LeakRemoteMsg> _MsgQueue = new ConcurrentQueue<LeakRemoteMsg>();

                private readonly int _MaxNum = 0;
                private readonly short _OutNum = 0;
                private readonly Action<LeakRemoteMsg> _Send = null;
                public LeakLimit(int size, short outNum, Action<LeakRemoteMsg> send, ILimitServer server)
                {
                        this._Send = send;
                        this._OutNum = outNum;
                        this._MaxNum = size;
                        server.AddEntrust(this._SendMsg);
                }
                public bool IsUsable => this._MsgQueue.Count < this._MaxNum;
                public bool IsLimit()
                {
                        return this._MsgQueue.Count > this._MaxNum;
                }
                public TcpRemoteReply MsgEvent(string key, TcpRemoteMsg msg, ISocketClient client)
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
                private void _SendMsg()
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
                public void Refresh(int time)
                {

                }

                public void Reset()
                {

                }
        }
}
