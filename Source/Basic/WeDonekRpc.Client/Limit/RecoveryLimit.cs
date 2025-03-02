using System.Threading;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Limit
{
    internal class RecoveryLimit : IServerLimit
    {
        private readonly int _OverTime = 0;

        private bool _IsInvalid = false;
        private int _SurplusNum = 0;
        public bool IsInvalid => this._IsInvalid;

        public bool IsUsable => Interlocked.CompareExchange(ref this._SurplusNum, 0, 0) > 0;

        public RecoveryLimit(int limitNum, int time)
        {
            this._SurplusNum = limitNum;
            this._OverTime = HeartbeatTimeHelper.HeartbeatTime + time;
        }
        public bool IsLimit()
        {
            return Interlocked.Decrement(ref this._SurplusNum) < 0;
        }
        public void Refresh(int time)
        {
            if (this._OverTime <= time)
            {
                this._IsInvalid = true;
            }
        }

        public void Reset()
        {
        }
    }
}
