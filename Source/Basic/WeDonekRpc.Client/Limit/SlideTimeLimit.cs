using System.Linq;
using System.Threading;

using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;

using WeDonekRpc.Model;

using WeDonekRpc.TcpServer.Interface;

using WeDonekRpc.Helper;
using WeDonekRpc.IOSendInterface;

namespace WeDonekRpc.Client.Limit
{
        /// <summary>
        /// 滑动时间
        /// </summary>
        internal class SlideTimeLimit : IMsgLimit
        {
                private readonly int _BeginTime = 0;
                //时间窗口
                private readonly int[] _SlideWin = null;
                private readonly int _LimitNum = 0;

                private int _SurplusNum = 0;
                private int _OneNum = 0;

                public SlideTimeLimit(int limitNum, short time)
                {
                        this._BeginTime = HeartbeatTimeHelper.HeartbeatTime;
                        this._SlideWin = new int[time];
                        this._OneNum = limitNum;
                        this._SurplusNum = limitNum;
                        this._LimitNum = limitNum;
                }
                public bool IsInvalid => false;
                public bool IsUsable => Interlocked.CompareExchange(ref this._SurplusNum, 0, 0) > 0;

                public bool IsLimit()
                {
                        return Interlocked.Decrement(ref this._SurplusNum) < 0;
                }
                public TcpRemoteReply MsgEvent(string key, TcpRemoteMsg msg, IIOClient client)
                {
                        if (this.IsLimit())
                        {
                                return new TcpRemoteReply(new BasicRes("rpc.exceed.limt"));
                        }
                        return RpcMsgCollect.MsgEvent(new Model.RemoteMsg(key, msg));
                }
                public void Refresh(int time)
                {
                        int num = time - this._BeginTime;
                        if (num == 0)
                        {
                                return;
                        }
                        num %= this._SlideWin.Length;
                        int begin;
                        if (num == 0)
                        {
                                begin = this._SlideWin.Length - 1;
                        }
                        else
                        {
                                begin = num - 1;
                        }
                        this._SlideWin[begin] = 0;
                        int surp = Interlocked.CompareExchange(ref this._SurplusNum, 0, 0);
                        this._SlideWin[num] = this._OneNum - surp;
                        this._OneNum = this._LimitNum - this._SlideWin.Sum();
                        Interlocked.Exchange(ref this._SurplusNum, this._OneNum);
                }

                public void Reset()
                {
                        for (int i = 0; i < this._SlideWin.Length; i++)
                        {
                                this._SlideWin[i] = 0;
                        }
                        Interlocked.Exchange(ref this._SurplusNum, this._LimitNum);
                }
        }
}

