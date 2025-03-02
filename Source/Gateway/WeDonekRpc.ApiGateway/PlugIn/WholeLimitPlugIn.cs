using System.Threading;
using WeDonekRpc.ApiGateway.Config;
using WeDonekRpc.ApiGateway.Helper;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Helper;

namespace WeDonekRpc.ApiGateway.PlugIn
{
    /// <summary>
    /// 全局限制
    /// </summary>
    internal class WholeLimitPlugIn : BasicPlugIn, IWholeLimitPlugIn
    {
        private readonly IWholeLimitConfig _Config;
        private ILimit _Limit;
        private Timer _Timer;

        public override bool IsEnable => this._Config.LimitType != GatewayLimitType.不启用;

        public WholeLimitPlugIn (IWholeLimitConfig config) : base("WholeLimit")
        {
            this._Config = config;
            this._Config.AddRefreshEvent(this._InitConfig);
        }

        private void _InitConfig (string name)
        {
            if (name != null)
            {
                base._ChangeEvent();
            }
            if (this._Limit != null && this._Limit.LimitType != this._Config.LimitType)
            {
                this._Limit = LimitHelper.GetLimit(this._Config);
                base._ChangeEvent();
            }
        }
        protected override void _Dispose ()
        {
            if (this._Config.LimitType == GatewayLimitType.不启用)
            {
                this._Timer?.Dispose();
                this._Timer = null;
                this._Limit = null;
            }
        }
        protected override void _InitPlugIn ()
        {
            if (this._Config.LimitType != GatewayLimitType.不启用)
            {
                if (this._Limit == null)
                {
                    this._Limit = LimitHelper.GetLimit(this._Config);
                }
                if (this._Timer == null)
                {
                    this._Timer = new Timer(this._Refresh, null, 1000, 1000);
                }
            }
        }

        private void _Refresh (object state)
        {
            if (this._Limit != null)
            {
                int now = HeartbeatTimeHelper.HeartbeatTime;
                this._Limit.Refresh(now);
            }
        }
        public bool IsLimit ()
        {
            if (this._Config.LimitType == GatewayLimitType.不启用)
            {
                return false;
            }
            return this._Limit.IsLimit();
        }
    }
}
