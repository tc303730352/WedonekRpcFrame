using RpcExtend.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcExtend.DAL.Repository
{
    internal class SystemLogDAL : ISystemLogDAL
    {
        private readonly IRepository<SystemErrorLog> _BasicDAL;
        public SystemLogDAL (IRepository<SystemErrorLog> dal)
        {
            this._BasicDAL = dal;
        }

        public void Adds (SystemErrorLog[] logs)
        {
            this._BasicDAL.Insert(logs);
        }
    }
}
