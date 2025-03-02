using RpcCentral.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcCentral.DAL.Repository
{
    internal class ServiceVerConfigDAL : IServiceVerConfigDAL
    {
        private readonly IRepository<ServiceVerConfig> _Db;
        public ServiceVerConfigDAL (IRepository<ServiceVerConfig> db)
        {
            this._Db = db;
        }

        public long GetVerId (int ver, long systemTypeId, long rpcMerId)
        {
            return this._Db.JoinGet<ServerPublicSchemeModel, long>((a, b) => a.SchemeId == b.Id && a.SystemTypeId == systemTypeId,
                (a, b) => b.RpcMerId == rpcMerId &&
                b.Status == SchemeStatus.启用 &&
                a.VerNum == ver, (a, b) => a.Id);
        }
    }
}
