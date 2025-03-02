using RpcCentral.Common;
using RpcCentral.DAL;
using RpcCentral.Model.DB;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Model.Server;

namespace RpcCentral.Collect.Collect
{
    internal class ReduceInRankCollect : IReduceInRankCollect
    {
        private readonly IReduceInRankConfigDAL _ReduceInRankConfig;
        private readonly ICacheController _Cache;
        public ReduceInRankCollect (IReduceInRankConfigDAL reduceInRankConfig, ICacheController cache)
        {
            this._Cache = cache;
            this._ReduceInRankConfig = reduceInRankConfig;
        }

        public ReduceInRank GetReduceInRank (long rpcMerId, long serverId)
        {
            string key = string.Format("Reduce_{0}_{1}", rpcMerId, serverId);
            if (this._Cache.TryGet(key, out ReduceInRank config))
            {
                return config;
            }
            ReduceInRankConfig data = this._ReduceInRankConfig.GetReduceInRank(rpcMerId, serverId);
            if (data == null)
            {
                config = new ReduceInRank();
            }
            else
            {
                config = data.ConvertMap<ReduceInRankConfig, ReduceInRank>();
            }
            _ = this._Cache.Set(key, config, new TimeSpan(10, 0, 0, 0));
            return config;
        }

        public void Refresh (long rpcMerId, long serverId)
        {
            string key = string.Format("Reduce_{0}_{1}", rpcMerId, serverId);
            _ = this._Cache.Remove(key);
        }
    }
}
