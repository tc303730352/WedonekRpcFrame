using System.Linq;
using System.Threading;
using WeDonekRpc.ApiGateway.Config;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Helper;

namespace WeDonekRpc.ApiGateway.Limit
{
        /// <summary>
        /// 滑动时间
        /// </summary>
        internal class SlideTimeLimit : ILimit
        {
                private readonly int _BeginTime = 0;
                //时间窗口
                private readonly int[] _SlideWin = null;
                private readonly int _LimitNum = 0;

                private int _SurplusNum = 0;
                private int _OneNum = 0;


                public GatewayLimitType LimitType => GatewayLimitType.流动时间窗;

                public SlideTimeLimit (LimitTimeWin limit)
                {
                        this._BeginTime = HeartbeatTimeHelper.HeartbeatTime;
                        this._SlideWin = new int[limit.Interval];
                        this._OneNum = limit.LimitNum;
                        this._SurplusNum = limit.LimitNum;
                        this._LimitNum = limit.LimitNum;
                }

                public bool IsLimit ()
                {
                        return Interlocked.Decrement (ref this._SurplusNum) < 0;
                }

                public void Refresh (int time)
                {
                        int num = time - this._BeginTime;
                        if (num == 0)
                        {
                                return;
                        }
                        num %= this._SlideWin.Length;
                        int begin = num == 0 ? this._SlideWin.Length - 1 : num - 1;
                        this._SlideWin[begin] = 0;
                        int surp = Interlocked.CompareExchange (ref this._SurplusNum, 0, 0);
                        this._SlideWin[num] = this._OneNum - surp;
                        this._OneNum = this._LimitNum - this._SlideWin.Sum ();
                        Interlocked.Exchange (ref this._SurplusNum, this._OneNum);
                }
        }
}

