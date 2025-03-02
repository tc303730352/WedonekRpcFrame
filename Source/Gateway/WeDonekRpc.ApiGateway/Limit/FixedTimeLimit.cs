using System.Threading;
using WeDonekRpc.ApiGateway.Config;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Helper;

namespace WeDonekRpc.ApiGateway.Limit
{
    internal class FixedTimeLimit : ILimit
    {
        private int _CurrentTime = 0;

        private readonly short _LimitTime = 0;
        private readonly int _LimitNum = 0;

        private int _SurplusNum = 0;

        public GatewayLimitType LimitType => GatewayLimitType.固定时间窗;

        public FixedTimeLimit(LimitTimeWin limit)
        {
            this._LimitTime = limit.Interval;
            this._LimitNum = limit.LimitNum;
            Interlocked.Exchange(ref this._SurplusNum, limit.LimitNum);
            this._CurrentTime = HeartbeatTimeHelper.HeartbeatTime + this._LimitTime;
        }
        public bool IsLimit()
        {
            return Interlocked.Decrement(ref this._SurplusNum) < 0;
        }

        public void Refresh(int time)
        {
            if (this._CurrentTime <= time)
            {
                Interlocked.Exchange(ref this._SurplusNum, this._LimitNum);
                this._CurrentTime = time + this._LimitTime;
            }
        }
    }
}
