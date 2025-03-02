using System.Threading;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Helper;
namespace WeDonekRpc.ApiGateway.IpLimit
{
    internal class LocalIpLimit : IIpLimit
    {
        private int _LimitNum;
        private int _LimitTime;
        private int _Surplus;
        private int _Time = 0;

        private int _Heartbeat = 0;
        public LocalIpLimit (string ip, IIpLimitConfig config)
        {
            this.Ip = ip;
            this._LimitNum = config.LimitNum;
            this._Surplus = config.LimitNum;
            this._LimitTime = config.LimitTime;
            this._Heartbeat = HeartbeatTimeHelper.HeartbeatTime;
            this._Time = this._Heartbeat + config.LimitTime;
        }
        public string Ip { get; }

        public bool IsLimit ()
        {
            return Interlocked.Add(ref this._Surplus, -1) < 0;
        }
        public int Refresh (int now)
        {
            if (now >= this._Time)
            {
                this._Time = now + this._LimitTime;
                if (Interlocked.Exchange(ref this._Surplus, this._LimitNum) != this._LimitNum)
                {
                    this._Heartbeat = HeartbeatTimeHelper.HeartbeatTime;
                }
            }
            return this._Heartbeat;
        }

        public void Reset (IIpLimitConfig config)
        {
            this._LimitNum = config.LimitNum;
            this._LimitTime = config.LimitTime;
        }
    }
}
