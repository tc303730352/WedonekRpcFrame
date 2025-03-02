using RpcStore.Model.ExtendDB;
using RpcStore.Model.TraceLog;

namespace RpcStore.DAL.Repository
{
    internal class RpcTraceLogDAL : IRpcTraceLogDAL
    {
        private readonly IRpcExtendResource<RpcTraceLogModel> _BasicDAL;
        public RpcTraceLogDAL (IRpcExtendResource<RpcTraceLogModel> dal)
        {
            this._BasicDAL = dal;
        }

        public RpcTraceLog[] Gets (string traceId)
        {
            return this._BasicDAL.Gets<RpcTraceLog>(c => c.TraceId == traceId);
        }
        public RpcTraceLogModel Get (long id)
        {
            return this._BasicDAL.Get(c => c.Id == id);
        }
    }
}
