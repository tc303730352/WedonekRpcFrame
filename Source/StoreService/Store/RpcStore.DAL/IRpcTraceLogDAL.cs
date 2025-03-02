using RpcStore.Model.ExtendDB;
using RpcStore.Model.TraceLog;

namespace RpcStore.DAL
{
    public interface IRpcTraceLogDAL
    {
        RpcTraceLogModel Get(long id);
        RpcTraceLog[] Gets(string traceId);
    }
}