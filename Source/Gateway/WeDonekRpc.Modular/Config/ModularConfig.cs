using WeDonekRpc.Client;
using WeDonekRpc.ModularModel.Resource;

namespace WeDonekRpc.Modular.Config
{
    internal class ModularConfig
    {
        private static readonly IAccreditConfig _Config;
        private static readonly IdentityConfig _Identity;
        static ModularConfig ()
        {
            _Identity = new IdentityConfig();
            _Config = new AccreditConfig();
            UpRange = RpcClient.Config.GetConfigVal("rpcassembly:Resource:UpRange", (ResourceType)6);
        }
        /// <summary>
        /// 授权配置
        /// </summary>
        public static IAccreditConfig Accredit => _Config;
        /// <summary>
        /// 资源上传范围
        /// </summary>
        public static ResourceType UpRange
        {
            get;
            private set;
        }
        /// <summary>
        /// 授权角色类型
        /// </summary>
        public static string AccreditRoleType => _Config.RoleType;

        public static IdentityConfig GetIdentityConfig ()
        {
            return _Identity;
        }
    }
}
