using RpcExtend.Model.DB;
using WeDonekRpc.Helper;
using WeDonekRpc.SqlSugar;

namespace RpcExtend.DAL.Repository
{
    internal class RpcTraceListDAL : IRpcTraceListDAL
    {
        private IRepository<RpcTrace> _BasicDAL;
        public RpcTraceListDAL(IRepository<RpcTrace> dal)
        {
            _BasicDAL = dal;
        }
        public void Adds(RpcTrace[] traces)
        {
            _BasicDAL.Insert(traces);
        }
    }
}
