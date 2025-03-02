using RpcCentral.Collect;
using RpcCentral.Common;
using RpcCentral.Service.RpcEvent;
using RpcCentral.Service.TcpService;
using WeDonekRpc.CacheClient;
using WeDonekRpc.CacheClient.Config;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.SqlSugar;

namespace RpcCentral.Service
{
    public class CentralService
    {
        public static void Start ()
        {
            IdentityHelper.Init(RpcContralConfig.GetIdentityConfig());
            CacheConfig config = RpcContralConfig.GetCache();
            RpcCacheService.SetVerNum(RpcContralConfig.CacheVerNum);
            RpcCacheService.Init(config, RpcContralConfig.CacheType);
            UnityHelper.Init(a =>
            {
                RpcCacheService.Load(new CacheIocContainer(a));
                SqlSugarService.Init(new SqlSugarContainer(a));
                RpcEventService.InitEvent(a);
                TcpRouteService.InitRoute(a);
            });
            IRpcServerCollect server = UnityHelper.Resolve<IRpcServerCollect>();
            server.LoadServer();
            TcpService.TcpService.Start();
        }
    }
}
