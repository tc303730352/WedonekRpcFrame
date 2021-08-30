
using RpcClient.Interface;

using RpcModular.Model;

namespace RpcModular.Config
{
        internal class ModularConfig
        {

                static ModularConfig()
                {
                        RpcClient.RpcClient.Config.AddRefreshEvent(_Refresh);
                }

                /// <summary>
                /// 授权配置
                /// </summary>
                public static AccreditConfig Accredit
                {
                        get;
                        private set;
                }
                /// <summary>
                /// 授权角色类型
                /// </summary>
                public static string AccreditRoleType { get; private set; }

                private static void _Refresh(IConfigServer config, string name)
                {
                        if (name.StartsWith("accredit") || name == string.Empty)
                        {
                                ModularConfig.Accredit = config.GetConfigVal<AccreditConfig>("accredit", new AccreditConfig());
                                ModularConfig.AccreditRoleType = config.GetConfigVal("accredit:RoleType", RpcClient.RpcClient.CurrentSource.GroupTypeVal);
                        }
                }
        }
}
