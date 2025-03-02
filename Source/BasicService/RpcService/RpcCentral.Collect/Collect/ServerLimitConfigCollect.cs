using RpcCentral.DAL;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Model.Model;

namespace RpcCentral.Collect.Collect
{
    internal class ServerLimitConfigCollect : IServerLimitConfigCollect
    {
        private readonly IServerLimitConfigDAL _BasicDAL;
        private readonly ICacheController _Cache;
        public ServerLimitConfigCollect (IServerLimitConfigDAL dal, ICacheController cache)
        {
            this._Cache = cache;
            this._BasicDAL = dal;
        }
        public void Refresh (long serverId)
        {
            string key = string.Concat("Limit_", serverId);
            _ = this._Cache.Remove(key);
        }
        public ServerLimitConfig GetServerLimit (long serverId)
        {
            string key = string.Concat("Limit_", serverId);
            if (this._Cache.TryGet(key, out ServerLimitConfig config))
            {
                return config;
            }
            config = this._BasicDAL.GetLimitConfig(serverId);
            if (config == null)
            {
                config = new ServerLimitConfig
                {
                    IsEnable = false,
                    IsEnableBucket = false
                };
            }
            _ = this._Cache.Add(key, config, new TimeSpan(10, 0, 0, 0));
            return config;
        }
    }
}
