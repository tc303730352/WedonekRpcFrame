using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.Trace.Model;

namespace RpcStore.DAL
{
    public interface IRpcTraceListDAL
    {
        RpcTraceModel[] Query(TraceQuery query, IBasicPage paging, out int count);
    }
}