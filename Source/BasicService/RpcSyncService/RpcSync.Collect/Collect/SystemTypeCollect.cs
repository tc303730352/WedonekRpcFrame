using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Model;
using RpcSync.DAL;
using RpcSync.Model;

namespace RpcSync.Collect.Collect
{
    internal class SystemTypeCollect : ISystemTypeCollect
    {
        private readonly IRemoteServerTypeDAL _SysTypeDAL;
        private readonly ILocalCacheController _Cache;
        public SystemTypeCollect (IRemoteServerTypeDAL sysTypeDAL, ILocalCacheController cache)
        {
            this._Cache = cache;
            this._SysTypeDAL = sysTypeDAL;
        }

        public string GetName (string sysType)
        {
            string key = "SysType_" + sysType;
            if (this._Cache.TryGet(key, out string name))
            {
                return name;
            }
            name = this._SysTypeDAL.GetName(sysType);
            _ = this._Cache.Set(key, name, DateTime.Now.AddDays(1));
            return name;
        }

        public RpcServerType GetServerType (long systemTypeId)
        {
            string key = "ServerType_" + systemTypeId;
            if (this._Cache.TryGet(key, out RpcServerType type))
            {
                return type;
            }
            type = this._SysTypeDAL.GetServerType(systemTypeId);
            _ = this._Cache.Set(key, type, DateTime.Now.AddDays(1));
            return type;
        }

        public long GetSystemTypeId (string systemType)
        {
            return this._SysTypeDAL.GetSystemTypeId(systemType);
        }

        public SystemType[] GetSystemTypes ()
        {
            return this._SysTypeDAL.GetSystemType();
        }

        public string[] GetSystemTypeVals ()
        {
            return this._SysTypeDAL.GetSystemTypeVals();
        }

        public Dictionary<long, string> GetSystemTypeVals (long[] ids)
        {
            return this._SysTypeDAL.GetSystemTypeVals(ids);
        }
    }
}
