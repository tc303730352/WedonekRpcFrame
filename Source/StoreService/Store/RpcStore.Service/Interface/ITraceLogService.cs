using RpcStore.RemoteModel.TraceLog.Model;

namespace RpcStore.Service.Interface
{
    public interface ITraceLogService
    {
        TraceLogDatum Get(long id);
        TraceLog[] Gets(string traceId);
    }
}