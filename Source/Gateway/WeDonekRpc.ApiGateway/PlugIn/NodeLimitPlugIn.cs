using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using WeDonekRpc.ApiGateway.Config;
using WeDonekRpc.ApiGateway.Helper;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Helper;

namespace WeDonekRpc.ApiGateway.PlugIn
{
    /// <summary>
    /// 请求接口限流插件
    /// </summary>
    internal class NodeLimitPlugIn : BasicPlugIn, INodeLimitPlugIn
    {
        private readonly ConcurrentDictionary<string, ILimit> _NodeLimit = new ConcurrentDictionary<string, ILimit>();

        private readonly INodeLimitConfig _Config;
        private Timer _Timer;

        public override bool IsEnable => this._Config.LimitType != GatewayLimitType.不启用;

        public NodeLimitPlugIn (INodeLimitConfig config) : base("NodeLimit")
        {
            this._Config = config;
            this._Config.AddRefreshEvent(this._RefreshConfig);
        }

        private void _Refresh (object state)
        {
            if (!this._NodeLimit.IsEmpty)
            {
                int now = HeartbeatTimeHelper.HeartbeatTime;
                KeyValuePair<string, ILimit>[] limits = this._NodeLimit.ToArray();
                foreach (KeyValuePair<string, ILimit> i in limits)
                {
                    ILimit limit = i.Value;
                    limit.Refresh(now);
                }
            }
        }
        private void _RefreshConfig (string name)
        {
            if (name != null)
            {
                base._ChangeEvent();
            }
            if (!this._NodeLimit.IsEmpty && name == "LimitType")
            {
                this._NodeLimit.Clear();
            }
        }
        protected override void _InitPlugIn ()
        {
            if (this._Config.LimitType == GatewayLimitType.不启用)
            {
                return;
            }
            if (this._Timer == null)
            {
                this._Timer = new Timer(this._Refresh, null, 1000, 1000);
            }
        }

        protected override void _Dispose ()
        {
            if (this._Config.LimitType == GatewayLimitType.不启用)
            {
                this._Timer?.Dispose();
                this._NodeLimit.Clear();
                this._Timer = null;
            }
        }
        public bool IsLimit (string node)
        {
            if (!this._Config.CheckIsLimit(node))
            {
                return false;
            }
            else if (this._NodeLimit.TryGetValue(node, out ILimit limit))
            {
                return limit.IsLimit();
            }
            else
            {
                limit = LimitHelper.GetLimit(this._Config);
                _ = this._NodeLimit.TryAdd(node, limit);
                return limit.IsLimit();
            }

        }
    }
}
