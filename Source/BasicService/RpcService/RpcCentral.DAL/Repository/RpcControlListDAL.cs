using RpcCentral.Model;
using RpcCentral.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcCentral.DAL.Repository
{
    internal class RpcControlListDAL : IRpcControlListDAL
    {
        private readonly IRepository<RpcControlList> _Db;
        public RpcControlListDAL (IRepository<RpcControlList> db)
        {
            this._Db = db;
        }
        public RpcControl[] GetControlServer ()
        {
            return this._Db.GetAll<RpcControl>();
        }
    }
}
