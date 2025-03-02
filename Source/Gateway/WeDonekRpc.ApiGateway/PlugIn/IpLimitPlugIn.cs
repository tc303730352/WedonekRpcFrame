using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.ApiGateway.IpLimit;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Helper;

namespace WeDonekRpc.ApiGateway.PlugIn
{
    /// <summary>
    /// IP限流
    /// </summary>
    internal class IpLimitPlugIn : BasicPlugIn, IIpLimitPlugIn
    {
        private readonly ConcurrentDictionary<string, IIpLimit> _IpLimit = new ConcurrentDictionary<string, IIpLimit>();
        private readonly IIpLimitConfig _Config;
        private readonly IRedisStringController _Cache;
        private Timer _Timer;
        public IpLimitPlugIn (IRedisStringController cache, IIpLimitConfig config) : base("IpLimit")
        {
            this._Cache = cache;
            this._Config = config;
            config.AddRefreshEvent(this._RefreshConfig);
        }
        public override bool IsEnable => this._Config.IsEnable;


        protected override void _InitPlugIn ()
        {
            if (this._Config.IsEnable)
            {
                if (this._Timer == null)
                {
                    this._Timer = new Timer(this._Refresh, null, 2000, 5000);
                }
            }
        }
        private void _Refresh (object state)
        {
            if (!this._IpLimit.IsEmpty)
            {
                int now = HeartbeatTimeHelper.HeartbeatTime;
                IIpLimit[] limits = this._IpLimit.Values.ToArray();
                limits.ForEachByParallel(a => a.Refresh(now));
            }
        }
        private void _RefreshConfig (string name)
        {
            if (name != null)
            {
                this._ChangeEvent();
            }
            if (!this._Config.IsEnable)
            {
                this._IpLimit.Clear();
                return;
            }
            if (name == "IsLocal")
            {
                this._IpLimit.Clear();
            }
            else if (!this._IpLimit.IsEmpty)
            {
                IIpLimit[] limits = this._IpLimit.Values.ToArray();
                limits.ForEachByParallel(a => a.Reset(this._Config));
            }
        }

        public bool IsLimit (string ip)
        {
            if (this._IpLimit.TryGetValue(ip, out IIpLimit limit))
            {
                return limit.IsLimit();
            }
            limit = this._Get(ip);
            if (this._IpLimit.TryAdd(ip, limit))
            {
                return limit.IsLimit();
            }
            return this.IsLimit(ip);
        }
        private IIpLimit _Get (string ip)
        {
            if (this._Config.IsLocal)
            {
                return new LocalIpLimit(ip, this._Config);
            }
            else
            {
                return new RemoteIpLimit(ip, this._Config, this._Cache);
            }

        }

        protected override void _Dispose ()
        {
            if (!this._Config.IsEnable)
            {
                this._Timer?.Dispose();
                this._IpLimit.Clear();
                this._Timer = null;
            }
        }
    }
}
