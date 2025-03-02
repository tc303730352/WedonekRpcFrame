using RpcCentral.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcCentral.DAL.Repository
{
    internal class ServiceVerRouteDAL : IServiceVerRouteDAL
    {
        private readonly IRepository<ServiceVerRoute> _Db;
        public ServiceVerRouteDAL (IRepository<ServiceVerRoute> db)
        {
            this._Db = db;
        }


        public Dictionary<long, int> GetVerRoute (long verId)
        {
            return this._Db.Gets(a => a.VerId == verId, a => new
            {
                a.VerId,
                a.ToVerId
            }).ToDictionary(a => a.VerId, a => a.ToVerId);
        }
    }
}
