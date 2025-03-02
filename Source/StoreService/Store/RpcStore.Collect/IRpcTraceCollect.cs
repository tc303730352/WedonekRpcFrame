using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.Trace.Model;

namespace RpcStore.Collect
{
    public interface IRpcTraceCollect
    {
        RpcTraceModel[] Query(TraceQuery query, IBasicPage paging, out int count);
    }
}