using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.ApiGateway.Config
{
    public enum IdentityReadMode
    {
        无 = 0,
        Head = 1,
        GET = 2,
        Accredit = 3
    }
    /// <summary>
    /// 网关身份应用标识
    /// </summary>
    internal class GatewayIdentityConfig : IGatewayIdentityConfig
    {
        public GatewayIdentityConfig ()
        {
            RpcClient.Config.GetSection("gateway:Identity").AddRefreshEvent(this._Refresh);
        }

        private void _Refresh (IConfigSection section, string name)
        {
            this.ReadMode = section.GetValue("ReadMode", IdentityReadMode.Head);
            this.ParamName = section.GetValue("ParamName", "identityId");
        }

        /// <summary>
        /// 身份标识读取方式
        /// </summary>
        public IdentityReadMode ReadMode
        {
            get;
            private set;
        }
        /// <summary>
        /// 默认应用Id
        /// </summary>
        public string ParamName
        {
            get;
            private set;
        }
    }
}
