using WeDonekRpc.Model;
using RpcSync.Model;
using RpcSync.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcSync.DAL.Repository
{
    internal class RemoteServerTypeDAL : IRemoteServerTypeDAL
    {
        private readonly IRepository<RemoteServerTypeModel> _BasicDAL;
        public RemoteServerTypeDAL (IRepository<RemoteServerTypeModel> dal)
        {
            this._BasicDAL = dal;
        }
        public long GetSystemTypeId (string sysType)
        {
            return this._BasicDAL.Get(c => c.TypeVal == sysType, c => c.Id);
        }
        public SystemType[] GetSystemType ()
        {
            return this._BasicDAL.GetAll<SystemType>();
        }
        public string[] GetSystemTypeVals ()
        {
            return this._BasicDAL.GetAll(c => c.TypeVal);
        }

        public Dictionary<long, string> GetSystemTypeVals (long[] ids)
        {
            return this._BasicDAL.Gets(a => ids.Contains(a.Id), a => new
            {
                a.Id,
                a.TypeVal
            }).ToDictionary(a => a.Id, a => a.TypeVal);
        }

        public RpcServerType GetServerType (long systemTypeId)
        {
            return this._BasicDAL.Get(a => a.Id == systemTypeId, a => a.ServiceType);
        }

        public string GetName (string sysType)
        {
            return this._BasicDAL.Get(a => a.TypeVal == sysType, a => a.SystemName);
        }
    }
}
