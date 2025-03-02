
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.ApiGateway.Config
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class GatewayConfig : IGatewayConfig
    {
        private static DefConfigParam _DefConfig = null;


        static GatewayConfig ()
        {
            RpcClient.Config.GetSection("gateway:trace").AddRefreshEvent(_Refresh);
        }

        private static void _Refresh (IConfigSection config, string name)
        {
            _DefConfig = config.GetValue(new DefConfigParam());
        }
        /// <summary>
        /// 前端强制开起链路的密码
        /// </summary>
        public string TracePwd => _DefConfig.TracePwd;
    }
}
