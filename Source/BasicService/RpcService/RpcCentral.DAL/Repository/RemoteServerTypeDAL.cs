using RpcCentral.Model;
using RpcCentral.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcCentral.DAL.Repository
{
    internal class RemoteServerTypeDAL : IRemoteServerTypeDAL
    {
        private readonly IRepository<RemoteServerType> _Db;
        public RemoteServerTypeDAL (IRepository<RemoteServerType> db)
        {
            this._Db = db;
        }
        public SystemTypeDatum GetSystemType (string typeVal)
        {
            return this._Db.Get<SystemTypeDatum>(c => c.TypeVal == typeVal);
        }
        public long GetSystemTypeId (string typeVal)
        {
            return this._Db.Get(c => c.TypeVal == typeVal, c => c.Id);
        }
    }
}
