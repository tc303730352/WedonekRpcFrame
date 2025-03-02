using System;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.ApiGateway.Config
{
    public enum NodeLimitRange
    {
        全局 = 0,
        配置 = 1
    }
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class NodeLimitConfig : LimitConfig, INodeLimitConfig
    {
        private Action<string> _Refresh;
        public NodeLimitConfig (IConfigCollect config)
        {
            IConfigSection section = config.GetSection("gateway:nodeLimit");
            section.AddRefreshEvent(this._Init);
        }

        private void _Init (IConfigSection section, string name)
        {
            this.LimitRange = section.GetValue<NodeLimitRange>("LimitRange", NodeLimitRange.全局);
            this.LimitType = section.GetValue<GatewayLimitType>("LimitType", GatewayLimitType.不启用);
            this.FixedTime = section.GetValue<LimitTimeWin>("FixedTime", new LimitTimeWin());
            this.FlowTime = section.GetValue<LimitTimeWin>("FlowTime", new LimitTimeWin());
            this.Token = section.GetValue<TokenConfig>("Token", new TokenConfig());
            this.Excludes = section.GetValue("Excludes", Array.Empty<string>());
            this.Limits = section.GetValue("Limits", Array.Empty<string>());
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
        /// <summary>
        /// 限定范围
        /// </summary>
        public NodeLimitRange LimitRange { get; set; }
        /// <summary>
        /// 排除的API路径
        /// </summary>
        public string[] Excludes { get; set; }
        /// <summary>
        /// 限定的API路径
        /// </summary>
        public string[] Limits { get; set; }
    }
}
