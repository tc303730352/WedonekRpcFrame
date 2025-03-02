using WeDonekRpc.CacheClient;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;

namespace WeDonekRpc.CacheModular
{
    [IocName("CacheService")]
    internal class CacheExtendService : IRpcExtendService
    {
        private readonly IRpcCacheConfig _Config;
        public CacheExtendService ( IRpcCacheConfig config )
        {
            this._Config = config;
        }
        public void Load ( IRpcService service )
        {
            service.InitComplating += this.Service_InitComplating;
        }

        private void Service_InitComplating ()
        {
            RpcCacheService.Init(this._Config.Cache, this._Config.CacheType);
        }
    }
}
