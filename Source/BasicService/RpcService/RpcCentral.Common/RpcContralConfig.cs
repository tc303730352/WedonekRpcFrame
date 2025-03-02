using RpcCentral.Common.Config;
using WeDonekRpc.CacheClient.Config;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.Helper.IdGenerator;
namespace RpcCentral.Common
{
    public class RpcContralConfig
    {
        private static readonly RpcServerSysConfig _SysConfig = null;
        static RpcContralConfig ()
        {
            TransmitScale = LocalConfig.Local.GetValue("rpc:TransmitRate", 2.5f);
            CacheType = LocalConfig.Local.GetValue("rpc:CacheType", CacheType.Redis);
            _SysConfig = LocalConfig.Local.GetValue<RpcServerSysConfig>("sys");
            TokenIsLocalSave = LocalConfig.Local.GetValue<bool>("rpc:token:isLocal", false);
        }
        public static CacheConfig GetCache ()
        {
            CacheConfig cache = new CacheConfig
            {
                SysKey = string.Concat("Rpc:", ServerIndex),
                Memcached = LocalConfig.Local.GetValue<MemcachedConfig>("memcached"),
                Redis = LocalConfig.Local.GetValue<RedisConfig>("redis"),
            };
            return cache;
        }
        public static IdentityConfig GetIdentityConfig ()
        {
            IdentityConfig identity = LocalConfig.Local.GetValue("rpc:Identity", new IdentityConfig());
            if (identity.WorkId == 0)
            {
                identity.WorkId = 1;
            }
            return identity;
        }
        public static int ServerIndex
        {
            get;
            private set;
        } = LocalConfig.Local.GetValue("rpc:ServerIndex", 0);

        public static int CacheVerNum
        {
            get;
            private set;
        }
        public static bool TokenIsLocalSave = false;
        public static float TransmitScale = 2.5f;
        public static CacheType CacheType { get; } = CacheType.Local;

        public static RpcServerSysConfig SysConfig => _SysConfig;
    }
}
