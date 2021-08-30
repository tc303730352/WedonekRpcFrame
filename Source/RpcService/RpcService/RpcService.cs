using RpcCacheClient;
using RpcCacheClient.Config;
using RpcCacheClient.Interface;

using RpcService.Collect;
using RpcService.Config;

using SqlExecHelper;

using RpcHelper;

namespace RpcService
{
        public class RpcService
        {
                internal static ICacheController Cache
                {
                        get;
                        private set;
                }
                public static void InitService()
                {
                        SqlExecHelper.Config.SqlConfig.SqlExecType = SqlExecType.全部;
                        CacheConfig cache = WebConfig.GetCache();
                        RpcCacheService.SetVerNum(WebConfig.CacheVerNum);
                        RpcCacheService.Init(cache, WebConfig.CacheType);
                        Cache = RpcCacheService.GetCache(WebConfig.CacheType);
                        TcpRouteCollect.InitRoute();
                        RpcServerCollect.Init();
                        new InfoLog("服务已启动!").Save();
                }
        }
}
