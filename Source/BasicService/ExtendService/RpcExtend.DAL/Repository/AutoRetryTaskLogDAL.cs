using RpcExtend.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcExtend.DAL.Repository
{
    internal class AutoRetryTaskLogDAL : IAutoRetryTaskLogDAL
    {
        private readonly IRepository<AutoRetryTaskLogModel> _BasicDAL;
        public AutoRetryTaskLogDAL (IRepository<AutoRetryTaskLogModel> dal)
        {
            this._BasicDAL = dal;
        }
        public void Adds (AutoRetryTaskLogModel[] adds)
        {
            this._BasicDAL.Insert(adds);
        }
    }
}
