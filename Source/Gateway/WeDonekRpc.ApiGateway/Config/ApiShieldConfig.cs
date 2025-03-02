using System;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.ApiGateway.Config
{
    /// <summary>
    /// 接口屏蔽配置
    /// </summary>
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class ApiShieldConfig : IApiShieldConfig
    {
        private Action<string> _Refresh;
        public ApiShieldConfig (IConfigCollect config)
        {
            IConfigSection section = config.GetSection("gateway:shieId");
            section.AddRefreshEvent(this._Init);
        }

        private void _Init (IConfigSection section, string name)
        {
            this.IsEnable = section.GetValue<bool>("IsEnable", true);
            this.IsLocal = section.GetValue<bool>("IsLocal", false);
            this.ShieIdPath = section.GetValue("ShieIdPath", Array.Empty<string>());
            if (this._Refresh != null)
            {
                this._Refresh(name);
            }
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable
        {
            get;
            private set;
        }
        /// <summary>
        /// 是否本地
        /// </summary>
        public bool IsLocal
        {
            get;
            private set;
        } = false;
        /// <summary>
        /// 屏蔽路径
        /// </summary>
        public string[] ShieIdPath
        {
            get;
            private set;
        }
        public void AddRefreshEvent (Action<string> action)
        {
            this._Refresh = action;
            action(null);
        }
    }
}
