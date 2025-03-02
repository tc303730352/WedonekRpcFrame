using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.Trace.Model;

namespace RpcStore.Service.Interface
{
    public interface IRpcTraceService
    {
        PagingResult<RpcTrace> Query(TraceQuery query, IBasicPage paging);
    }
}