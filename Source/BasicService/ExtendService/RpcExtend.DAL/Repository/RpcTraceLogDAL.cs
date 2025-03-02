using RpcExtend.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcExtend.DAL.Repository
{
    internal class RpcTraceLogDAL : IRpcTraceLogDAL
    {
        private IRepository<RpcTraceLog> _BasicDAL;
        public RpcTraceLogDAL(IRepository<RpcTraceLog> dal)
        {
            _BasicDAL = dal;
        }

        public void Adds(RpcTraceLog[] logs)
        {
            _BasicDAL.Insert(logs);
        }
    }
}
