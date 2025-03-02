using System;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.ApiGateway.Config
{
    internal class WholeLimitConfig : LimitConfig, IWholeLimitConfig
    {
        private Action<string> _Refresh;
        public WholeLimitConfig (IConfigCollect config)
        {
            IConfigSection section = config.GetSection("gateway:wholeLimit");
            section.AddRefreshEvent(this._Init);
        }

        private void _Init (IConfigSection section, string name)
        {
            this.LimitType = section.GetValue<GatewayLimitType>("LimitType", GatewayLimitType.不启用);
            this.FixedTime = section.GetValue<LimitTimeWin>("FixedTime", new LimitTimeWin());
            this.FlowTime = section.GetValue<LimitTimeWin>("FlowTime", new LimitTimeWin());
            this.Token = section.GetValue<TokenConfig>("Token", new TokenConfig());
            if (this._Refresh != null)
            {
                this._Refresh(name);
            }
        }
        public void AddRefreshEvent (Action<string> action)
        {
            this._Refresh = action;
            action(null);
        }
    }
}
