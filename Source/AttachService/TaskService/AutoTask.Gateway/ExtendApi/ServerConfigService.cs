using AutoTask.Gateway.Interface;
using RpcStore.RemoteModel.ServerConfig;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper;

namespace AutoTask.Gateway.ExtendApi
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class ServerConfigService : IServerConfigService
    {
        private readonly ICacheController _Cache;

        public ServerConfigService (ICacheController cache)
        {
            this._Cache = cache;
        }

        public string GetName (long id)
        {
            string key = "ServerName_" + id;
            if (this._Cache.TryGet(key, out string name))
            {
                return name;
            }
            name = new GetServerName
            {
                ServerId = id
            }.Send();
            _ = this._Cache.Add(key, name);
            return name;
        }

        public Dictionary<long, string> GetNames (long[] ids)
        {
            Array.Sort(ids);
            string key = "ServerName_" + ids.Join(',').GetMd5();
            if (this._Cache.TryGet(key, out Dictionary<long, string> name))
            {
                return name;
            }
            name = new GetServerNames
            {
                ServerId = ids
            }.Send();
            _ = this._Cache.Add(key, name);
            return name;
        }
    }
}
