using AutoTask.Gateway.Interface;
using RpcStore.RemoteModel.ServerType;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Client.Attr;

namespace AutoTask.Gateway.ExtendApi
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class ServerTypeService : IServerTypeService
    {
        private readonly ICacheController _Cache;

        public ServerTypeService (ICacheController cache)
        {
            this._Cache = cache;
        }

        public string GetName (string typeVal)
        {
            string key = "ServerTypeName_" + typeVal;
            if (this._Cache.TryGet(key, out string name))
            {
                return name;
            }
            name = new GetServerTypeNameByTypeVal
            {
                TypeVal = typeVal
            }.Send();
            _ = this._Cache.Add(key, name);
            return name;
        }
    }
}
