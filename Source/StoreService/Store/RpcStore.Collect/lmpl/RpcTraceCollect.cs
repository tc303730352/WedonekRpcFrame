using WeDonekRpc.Model;
using RpcStore.DAL;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.Trace.Model;

namespace RpcStore.Collect.lmpl
{
    internal class RpcTraceCollect : IRpcTraceCollect
    {
        private IRpcTraceListDAL _BasicDAL;

        public RpcTraceCollect(IRpcTraceListDAL basicDAL)
        {
            _BasicDAL = basicDAL;
        }

        public RpcTraceModel[] Query(TraceQuery query, IBasicPage paging, out int count)
        {
            return this._BasicDAL.Query(query, paging, out count);
        }
    }
}
