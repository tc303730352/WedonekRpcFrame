using System.Text;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Helper;
using RpcSync.DAL;
using RpcSync.Model;

namespace RpcSync.Collect.Collect
{
    internal class RemoteServerCollect : IRemoteServerCollect
    {
        private readonly IRemoteServerConfigDAL _BasicDAL;
        private readonly ILocalCacheController _Cache;
        public RemoteServerCollect (IRemoteServerConfigDAL basicDAL, ILocalCacheController cache)
        {
            this._BasicDAL = basicDAL;
            this._Cache = cache;
        }
        public long[] GetAllServer (int? regionId)
        {
            RemoteServer[] server = this._GetAllServer();
            if (!regionId.HasValue)
            {
                return server.ConvertAll(a => a.Id);
            }
            else
            {
                return server.Convert(a => a.RegionId == regionId, a => a.Id);
            }
        }
        public RemoteServer[] GetAllServers ()
        {
            return this._GetAllServer();
        }

        public RemoteServerConfig GetServer (long serverId)
        {
            string key = "Service_" + serverId;
            if (!this._Cache.TryGet(key, out RemoteServerConfig config))
            {
                config = this._BasicDAL.GetServer(serverId);
                if (config == null)
                {
                    config = new RemoteServerConfig();
                }
                _ = this._Cache.Add(key, config);
            }
            if (config.ServerId == 0)
            {
                throw new ErrorException("rpc.service.not.find");
            }
            return config;
        }
        public RemoteServer[] GetContainerServer (long[] containerId)
        {
            RemoteServer[] server = this._GetAllServer();
            return server.FindAll(c => c.IsContainer && containerId.Contains(c.ContainerId.Value));
        }
        public long[] GetServerId (int? regionId, List<long> sysTypeId)
        {
            RemoteServer[] server = this._GetAllServer();
            if (!regionId.HasValue)
            {
                return server.Convert(a => sysTypeId.IsExists(a.SystemType), a => a.Id);
            }
            else
            {
                return server.Convert(a => a.RegionId == regionId && sysTypeId.IsExists(a.SystemType), a => a.Id);
            }
        }
        private RemoteServer[] _GetAllServer ()
        {
            string key = "AllServer";
            if (this._Cache.TryGet(key, out RemoteServer[] servers))
            {
                return servers;
            }
            servers = this._BasicDAL.GetServer();
            _ = this._Cache.Set(key, servers, new TimeSpan(0, 0, Tools.GetRandom(60, 180)));
            return servers;
        }
    }
}
