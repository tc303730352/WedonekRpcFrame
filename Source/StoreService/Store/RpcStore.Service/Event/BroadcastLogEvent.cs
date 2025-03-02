using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.BroadcastErrorLog;
using RpcStore.RemoteModel.BroadcastErrorLog.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class BroadcastLogEvent : IRpcApiService
    {
        private IBroadcastLogService _Service;

        public BroadcastLogEvent(IBroadcastLogService service)
        {
            _Service = service;
        }

        public BroadcastLog GetBroadcastErrorLog(GetBroadcastErrorLog obj)
        {
            return _Service.Get(obj.Id);
        }
        public PagingResult<BroadcastLogDatum> QueryBroadcastLog(QueryBroadcastLog query)
        {
            return _Service.Query(query.Query, query.Lang, query.ToBasicPage());
        }
    }
}
