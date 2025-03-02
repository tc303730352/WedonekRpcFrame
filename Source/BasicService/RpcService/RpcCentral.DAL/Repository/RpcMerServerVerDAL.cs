using RpcCentral.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcCentral.DAL.Repository
{
    internal class RpcMerServerVerDAL : IRpcMerServerVerDAL
    {
        private readonly IRepository<RpcMerServerVerModel> _Db;
        public RpcMerServerVerDAL (IRepository<RpcMerServerVerModel> repository)
        {
            this._Db = repository;
        }

        public int? GetCurrentVer (long rpcMerId, long systemTypeId)
        {
            var cur = this._Db.Get(c => c.RpcMerId == rpcMerId && c.SystemTypeId == systemTypeId, c => new
            {
                c.CurrentVer
            });
            if (cur == null)
            {
                return null;
            }
            return cur.CurrentVer;
        }
    }
}
