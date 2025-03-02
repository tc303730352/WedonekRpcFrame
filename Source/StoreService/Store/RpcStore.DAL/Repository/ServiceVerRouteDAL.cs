using RpcStore.DAL;
using RpcStore.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class ServiceVerRouteDAL : IServiceVerRouteDAL
    {
        private readonly IRepository<ServiceVerRouteModel> _BasicDAL;
        public ServiceVerRouteDAL ( IRepository<ServiceVerRouteModel> dal )
        {
            this._BasicDAL = dal;
        }
        public ServiceVerRouteModel[] Gets ( long schemeId )
        {
            return this._BasicDAL.Gets(c => c.SchemeId == schemeId);
        }
    }
}
