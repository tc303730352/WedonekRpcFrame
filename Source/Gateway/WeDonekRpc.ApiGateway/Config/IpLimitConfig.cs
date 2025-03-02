using System;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.ApiGateway.Config
{
    /// <summary>
    /// IP限流配置
    /// </summary>
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class IpLimitConfig : IIpLimitConfig
    {
        private Action<string> _Refresh;
        public IpLimitConfig (IConfigCollect config)
        {
            IConfigSection section = config.GetSection("gateway:ipLimit");
            section.AddRefreshEvent(this._Init);
        }

        private void _Init (IConfigSection section, string name)
        {
            this.IsEnable = section.GetValue<bool>("IsEnable", false);
            this.IsLocal = section.GetValue<bool>("IsLocal", true);
            this.LimitTime = section.GetValue("LimitTime", 2);
            this.LimitNum = section.GetValue("LimitNum", 500);
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
        /// 是否启用
        /// </summary>
        public bool IsEnable
        {
            get;
            private set;
        } = false;
        /// <summary>
        /// 是否使用本地限流
        /// </summary>
        public bool IsLocal
        {
            get;
            private set;
        } = true;
        /// <summary>
        /// 时间窗大小(秒)
        /// </summary>
        public int LimitTime
        {
            get;
            private set;
        } = 2;
        /// <summary>
        /// 限制的请求数
        /// </summary>
        public int LimitNum
        {
            get;
            private set;
        } = 100;
    }
}
