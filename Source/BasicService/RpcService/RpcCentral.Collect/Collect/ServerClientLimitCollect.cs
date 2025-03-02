using RpcCentral.DAL;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Model;
using WeDonekRpc.Model.Model;

namespace RpcCentral.Collect.Collect
{
    internal class ServerClientLimitCollect : IServerClientLimitCollect
    {
        private readonly IServerClientLimitDAL _ServerClientLimit;
        private readonly ICacheController _Cache;
        public ServerClientLimitCollect (IServerClientLimitDAL serverClientLimit, ICacheController cache)
        {
            this._Cache = cache;
            this._ServerClientLimit = serverClientLimit;
        }

        public void Refresh (long rpcMerId, long serverId)
        {
            string key = string.Join("_", "ClientLimit", rpcMerId, serverId);
            _ = this._Cache.Remove(key);

        }
        public ServerClientLimit GetClientLimit (long rpcMerId, long serverId)
        {
            string key = string.Join("_", "ClientLimit", rpcMerId, serverId);
            if (this._Cache.TryGet(key, out ServerClientLimit config))
            {
                return config;
            }
            config = this._ServerClientLimit.GetClientLimit(rpcMerId, serverId);
            if (config == null)
            {
                config = new ServerClientLimit
                {
                    LimitType = ServerLimitType.不启用
                };
            }
            _ = this._Cache.Add(key, config, new TimeSpan(10, 0, 0, 0));
            return config;
        }
    }
}
