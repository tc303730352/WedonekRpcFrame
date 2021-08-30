
using ApiGateway.Interface;
using ApiGateway.Model;

using RpcClient.Interface;

namespace ApiGateway.Config
{
        internal class GatewayConfig : IGatewayConfig
        {
                private static DefConfigParam _DefConfig = null;


                static GatewayConfig()
                {
                        RpcClient.RpcClient.Config.AddRefreshEvent(_Refresh);
                }

                private static void _Refresh(IConfigServer config,string name)
                {
                        if (name.StartsWith("gateway") || name == string.Empty)
                        {
                                _DefConfig = config.GetConfigVal("gateway", new DefConfigParam());
                        }
                }
                /// <summary>
                /// 前端强制开起链路的密码
                /// </summary>
                public string TracePwd => _DefConfig.TracePwd;
        }
}
