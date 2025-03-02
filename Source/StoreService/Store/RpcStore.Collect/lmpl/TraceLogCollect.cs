using WeDonekRpc.Helper;
using RpcStore.DAL;
using RpcStore.Model.ExtendDB;
using RpcStore.Model.TraceLog;

namespace RpcStore.Collect.lmpl
{
    internal class TraceLogCollect : ITraceLogCollect
    {
        private readonly IRpcTraceLogDAL _BasicDAL;
        public TraceLogCollect (IRpcTraceLogDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }

        public RpcTraceLog[] Gets (string traceId)
        {
            return this._BasicDAL.Gets(traceId);
        }
        public RpcTraceLogModel Get (long id)
        {
            RpcTraceLogModel detailed = this._BasicDAL.Get(id);
            if (detailed != null)
            {
                return detailed;
            }
            throw new ErrorException("rpc.store.trace.log.not.find");
        }
    }
}
