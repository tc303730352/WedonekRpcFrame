using WeDonekRpc.CacheClient.Interface;
using RpcCentral.DAL;
using RpcCentral.Model;
using WeDonekRpc.Helper;

namespace RpcCentral.Collect.Collect
{
    internal class RemoteServerGroupCollect : IServerGroupCollect
    {
        private readonly ICacheController _Cache;
        private readonly IRemoteServerGroupDAL _ServerGroup;
        public RemoteServerGroupCollect (ICacheController cache, IRemoteServerGroupDAL serverGroup)
        {
            this._Cache = cache;
            this._ServerGroup = serverGroup;
        }

        public RemoteConfig[] GetRemoteServer (long merId, long systemTypeId)
        {
            return this._GetRemoteServer(merId, systemTypeId);
        }
        private RemoteConfig[] _GetRemoteServer (long merId, long systemTypeId)
        {
            string key = string.Join("_", "Remote", merId, systemTypeId);
            if (this._Cache.TryGet(key, out RemoteConfig[] configs))
            {
                return configs;
            }
            RemoteConfig[] services = this._ServerGroup.GetRemoteServer(merId, systemTypeId);
            _ = this._Cache.Add(key, services, new TimeSpan(10, 0, 0, 0));
            return services;
        }
        public void ClearCache (long merId, long systemTypeId)
        {
            string key = string.Join("_", "Remote", merId, systemTypeId);
            _ = this._Cache.Remove(key);
            key = string.Concat("Remote_", systemTypeId);
            _ = this._Cache.Remove(key);
        }
        public long[] GetRemoteServerId (long merId, long systemTypeId)
        {
            RemoteConfig[] remotes = this._GetRemoteServer(merId, systemTypeId);
            if (remotes.IsNull())
            {
                throw new ErrorException("rpc.server.list.null");
            }
            return remotes.ConvertAll(a => a.ServerId);
        }
        public long[] GetRpcMer (long systemTypeId)
        {
            string key = string.Concat("Remote_", systemTypeId);
            if (this._Cache.TryGet(key, out long[] serverId))
            {
                return serverId;
            }
            serverId = this._ServerGroup.GetRpcMer(systemTypeId);
            _ = this._Cache.Add(key, serverId, new TimeSpan(10, 0, 0, 0));
            return serverId;
        }
    }
}
