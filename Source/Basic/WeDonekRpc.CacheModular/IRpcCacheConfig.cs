using WeDonekRpc.CacheClient.Config;
using WeDonekRpc.CacheClient.Interface;

namespace WeDonekRpc.CacheModular
{
    public interface IRpcCacheConfig
    {
        CacheType CacheType { get; }
        CacheConfig Cache { get; }
    }
}