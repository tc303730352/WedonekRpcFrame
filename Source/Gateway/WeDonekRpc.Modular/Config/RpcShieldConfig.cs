using System;
using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.Modular.Config
{
    internal class RpcShieldConfig : IRpcShieldConfig
    {
        private Action<IRpcShieldConfig> _Refresh;
        public RpcShieldConfig (IConfigCollect config)
        {
            IConfigSection section = config.GetSection("rpc:shieId");
            section.AddRefreshEvent(this._Init);
        }

        private void _Init (IConfigSection section, string name)
        {
            this.IsEnable = section.GetValue("IsEnable", false);
            this.IsLocal = section.GetValue("IsLocal", false);
            this.DirectList = section.GetValue("Direct", Array.Empty<string>());
            if (this._Refresh != null)
            {
                this._Refresh(this);
            }
        }

        public void AddRefreshEvent (Action<IRpcShieldConfig> action)
        {
            this._Refresh = action;
            action(this);
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; private set; }

        /// <summary>
        /// 是否本地
        /// </summary>
        public bool IsLocal { get; private set; }
        /// <summary>
        /// 屏蔽路径
        /// </summary>
        public string[] DirectList { get; private set; }
    }
}
