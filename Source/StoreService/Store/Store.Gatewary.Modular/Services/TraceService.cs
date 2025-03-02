using WeDonekRpc.Model;
using RpcStore.RemoteModel.Trace;
using RpcStore.RemoteModel.Trace.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class TraceService : ITraceService
    {
        public RpcTrace[] QueryTrace(TraceQuery query, IBasicPage paging, out int count)
        {
            return new QueryTrace
            {
                Query = query,
                Index = paging.Index,
                Size = paging.Size,
                NextId = paging.NextId,
                SortName = paging.SortName,
                IsDesc = paging.IsDesc
            }.Send(out count);
        }

    }
}
