using WeDonekRpc.Client;
using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.Modular.Config
{
    public class IdentityConfig
    {
        public IdentityConfig ()
        {
            RpcClient.Config.GetSection("rpcassembly:Identity").AddRefreshEvent(this._Refresh);
        }

        private void _Refresh (IConfigSection section, string name)
        {
            this.IsEnable = section.GetValue("IsEnable", false);
            this.DefAppId = section.GetValue("DefAppId");
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
        /// 默认应用Id
        /// </summary>
        public string DefAppId
        {
            get;
            private set;
        }
    }
}
