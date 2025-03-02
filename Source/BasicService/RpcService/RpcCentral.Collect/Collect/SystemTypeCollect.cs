using RpcCentral.DAL;
using RpcCentral.Model;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Helper;

namespace RpcCentral.Collect.Collect
{
    internal class SystemTypeCollect : ISystemTypeCollect
    {
        private readonly IRemoteServerTypeDAL _BasicDAL;
        private readonly ICacheController _Cache;
        public SystemTypeCollect (IRemoteServerTypeDAL basicDAL, ICacheController cache)
        {
            this._Cache = cache;
            this._BasicDAL = basicDAL;
        }
        public long GetSystemTypeId (string typeVal)
        {
            string key = string.Concat("SysTypeId_", typeVal);
            if (this._Cache.TryGet(key, out long id))
            {
                return id;
            }
            id = this._BasicDAL.GetSystemTypeId(typeVal);
            if (id == 0)
            {
                throw new ErrorException("rpc.systemType.not.find", new Dictionary<string, string>
                {
                      {"typeVal",typeVal }
                });
            }
            _ = this._Cache.Add(key, id, new TimeSpan(30, 0, 0, 0));
            return id;
        }
        public void Refresh (string type)
        {
            string key = string.Concat("SystemType_", type);
            _ = this._Cache.Remove(key);
        }
        public SystemTypeDatum GetSystemType (string type)
        {
            string key = string.Concat("SystemType_", type);
            if (this._Cache.TryGet(key, out SystemTypeDatum datum))
            {
                return datum;
            }
            datum = this._BasicDAL.GetSystemType(type);
            if (datum == null)
            {
                throw new ErrorException("rpc.systemType.not.find");
            }
            _ = this._Cache.Add(key, datum, new TimeSpan(0, Tools.GetRandom(10, 60), 0));
            return datum;
        }
    }
}
