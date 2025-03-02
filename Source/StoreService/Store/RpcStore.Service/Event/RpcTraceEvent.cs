using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.Trace;
using RpcStore.RemoteModel.Trace.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class RpcTraceEvent : IRpcApiService
    {
        private IRpcTraceService _Service;
        public RpcTraceEvent(IRpcTraceService service)
        {
            _Service = service;
        }
        public PagingResult<RpcTrace> QueryTrace(QueryTrace query)
        {
            return _Service.Query(query.Query, query.ToBasicPage());
        }
    }
}
