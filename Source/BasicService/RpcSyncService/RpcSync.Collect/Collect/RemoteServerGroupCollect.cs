using RpcSync.DAL;
using RpcSync.Model;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcSync.Collect.Collect
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class RemoteServerGroupCollect : IRemoteServerGroupCollect
    {
        private readonly IIocService _Unity;
        private readonly ILocalCacheController _Cache;
        public RemoteServerGroupCollect (IIocService unity, ILocalCacheController cache)
        {
            this._Unity = unity;
            this._Cache = cache;
        }
        public long[] GetHoldServerId (long merId)
        {
            return this._Unity.Resolve<IRemoteServerGroupDAL>().GetHoldServerId(merId);
        }
        public string[] GetServerTypeVal (long merId)
        {
            string key = string.Concat("ServerType_", merId);
            if (this._Cache.TryGet(key, out string[] servers))
            {
                return servers;
            }
            servers = this._Unity.Resolve<IRemoteServerGroupDAL>().GetServerTypeVal(merId).Distinct();
            _ = this._Cache.Set(key, servers);
            return servers;
        }
        public MerServer[] GetAllServer (long merId)
        {
            string key = string.Concat("Server_", merId);
            if (this._Cache.TryGet(key, out MerServer[] servers))
            {
                return servers;
            }
            servers = this._Unity.Resolve<IRemoteServerGroupDAL>().GetAllServer(merId);
            _ = this._Cache.Set(key, servers);
            return servers;
        }

        public void Refresh (long merId, long[] remoteId)
        {
            string key = string.Concat("Server_", merId);
            _ = this._Cache.Remove(key);
            key = string.Concat("ServerType_", merId);
            _ = this._Cache.Remove(key);
            if (remoteId.IsNull())
            {
                return;
            }
            key = string.Concat("IsBind_", merId);
            remoteId.ForEach(c =>
            {
                string t = key + "_" + c;
                _ = this._Cache.Remove(t);
            });
        }

        public bool CheckIsExists (long rpcMerId, MsgSource source)
        {
            string key = string.Join("_", "IsBind", rpcMerId, source.ServerId);
            if (this._Cache.TryGet(key, out bool isBind))
            {
                return isBind;
            }
            isBind = this._Unity.Resolve<IRemoteServerGroupDAL>().CheckIsExists(rpcMerId, source);
            _ = this._Cache.Set(key, isBind);
            return isBind;
        }
    }
}
