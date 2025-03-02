using RpcStore.RemoteModel.TraceLog;
using RpcStore.RemoteModel.TraceLog.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class TraceLogService : ITraceLogService
    {
        public TraceLog[] GetTraceByTraceId(string traceId)
        {
            return new GetTraceByTraceId
            {
                TraceId = traceId,
            }.Send();
        }

        public TraceLogDatum GetTraceLog(long id)
        {
            return new GetTraceLog
            {
                Id = id,
            }.Send();
        }

    }
}
