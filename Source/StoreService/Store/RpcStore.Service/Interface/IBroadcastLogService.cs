using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.BroadcastErrorLog.Model;

namespace RpcStore.Service.Interface
{
    public interface IBroadcastLogService
    {
        BroadcastLog Get (long id);
        PagingResult<BroadcastLogDatum> Query (BroadcastErrorQuery query, string lang, IBasicPage paging);
    }
}