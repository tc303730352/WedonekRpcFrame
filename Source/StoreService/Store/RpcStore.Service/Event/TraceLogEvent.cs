using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.TraceLog;
using RpcStore.RemoteModel.TraceLog.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class TraceLogEvent : IRpcApiService
    {
        private ITraceLogService _Service;

        public TraceLogEvent(ITraceLogService service)
        {
            _Service = service;
        }

        public TraceLogDatum GetTraceLog(GetTraceLog obj)
        {
            return _Service.Get(obj.Id);
        }

        public TraceLog[] GetTraceByTraceId(GetTraceByTraceId obj)
        {
            return _Service.Gets(obj.TraceId);
        }
    }
}
