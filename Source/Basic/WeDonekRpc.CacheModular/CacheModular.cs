using WeDonekRpc.CacheClient;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;

namespace WeDonekRpc.CacheModular
{
    public class CacheModular : IRpcInitModular
    {
        public void Init ( IIocService ioc )
        {

        }
        public void Load ( RpcInitOption option )
        {
            RpcCacheService.Load(new CacheIocContainer(option));
        }

        public void InitEnd ( IIocService ioc, IRpcService service )
        {
        }
    }
}