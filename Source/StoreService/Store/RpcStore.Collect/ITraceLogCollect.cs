using RpcStore.Model.ExtendDB;
using RpcStore.Model.TraceLog;

namespace RpcStore.Collect
{
    public interface ITraceLogCollect
    {
        RpcTraceLogModel Get(long id);
        RpcTraceLog[] Gets(string traceId);
    }
}