using System;
using System.Net;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Helper;

namespace WeDonekRpc.ApiGateway.IpLimit
{
    internal class RemoteIpLimit : IIpLimit
    {
        private int _LimitNum;
        private int _LimitTime;
        private readonly string _CacheKey;
        private readonly IRedisStringController _Cache;
        private int _Heartbeat = 0;
        public RemoteIpLimit (string ip, IIpLimitConfig limit, IRedisStringController cache)
        {
            this.Ip = ip;
            this._Cache = cache;
            this._Heartbeat = HeartbeatTimeHelper.HeartbeatTime;
            this._CacheKey = string.Concat("IpLimit_", IPAddress.Parse(ip).Address);
            this._LimitNum = limit.LimitNum;
            this._LimitTime = limit.LimitTime;
        }
        public string Ip { get; }

        public bool IsLimit ()
        {
            long num = this._Cache.Increment(this._CacheKey);
            if (num == 1)
            {
                _ = this._Cache.SetExpire(this._CacheKey, new TimeSpan(0, 0, this._LimitTime));
            }
            this._Heartbeat = HeartbeatTimeHelper.HeartbeatTime;
            return num > this._LimitNum;
        }

        public int Refresh (int now)
        {
            return this._Heartbeat;
        }
        public void Reset (IIpLimitConfig config)
        {
            this._LimitNum = config.LimitNum;
            this._LimitTime = config.LimitTime;
        }
    }
}
