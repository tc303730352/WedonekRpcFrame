using RpcCentral.DAL;
using RpcCentral.Model;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Helper;

namespace RpcCentral.Collect.Collect
{
    internal class RpcMerConfigCollect : IRpcMerConfigCollect
    {
        private readonly IRpcMerConfigDAL _MerConfig;
        private readonly ICacheController _Cache;
        public RpcMerConfigCollect (IRpcMerConfigDAL merConfig, ICacheController cache)
        {
            this._Cache = cache;
            this._MerConfig = merConfig;
        }

        public MerConfig GetConfig (long rpcMerId, long sysTypeId)
        {
            string key = string.Format("Config_{0}_{1}", rpcMerId, sysTypeId);
            if (this._Cache.TryGet(key, out MerConfig config))
            {
                return config;
            }
            config = this._MerConfig.GetConfig(rpcMerId, sysTypeId);
            if (config == null)
            {
                config = new MerConfig
                {
                    IsRegionIsolate = true,
                    IsolateLevel = false
                };
            }
            _ = this._Cache.Add(key, config, new TimeSpan(0, Tools.GetRandom(10, 30), 0));
            return config;
        }
    }
}
