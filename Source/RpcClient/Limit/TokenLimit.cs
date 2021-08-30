using System.Threading;

using RpcClient.Collect;
using RpcClient.Interface;

using RpcModel;

using SocketTcpServer.Interface;

namespace RpcClient.Limit
{
        /// <summary>
        /// 令牌桶
        /// </summary>
        internal class TokenLimit : IMsgLimit
        {

                private readonly short _InNum = 0;
                private readonly int _TokenSize = 0;

                private int _SurplusNum = 0;

                public TokenLimit(int size, short add)
                {
                        this._InNum = add;
                        this._TokenSize = size;
                        this._SurplusNum = size;
                }
                public bool IsUsable => Interlocked.CompareExchange(ref this._SurplusNum, 0, 0) > 0;
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
                        int num = Interlocked.CompareExchange(ref this._SurplusNum, 0, 0);
                        if (num == this._TokenSize)
                        {
                                return;
                        }
                        int add = this._InNum;
                        if (num > 0)
                        {
                                add += num;
                        }
                        if (add > this._TokenSize)
                        {
                                add = this._TokenSize;
                        }
                        if (Interlocked.CompareExchange(ref this._SurplusNum, add, num) != num)
                        {
                                this.Refresh(time);
                        }
                }

                public void Reset()
                {
                        Interlocked.Exchange(ref this._SurplusNum, this._TokenSize);
                }
        }
}
