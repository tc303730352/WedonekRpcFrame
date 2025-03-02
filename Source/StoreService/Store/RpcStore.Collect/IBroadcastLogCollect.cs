using WeDonekRpc.Model;
using RpcStore.Model.BroadcastErrorLog;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.BroadcastErrorLog.Model;

namespace RpcStore.Collect
{
    public interface IBroadcastLogCollect
    {
        BroadcastErrorLogModel Get (long id);
        BroadcastErrorLog[] Query (BroadcastErrorQuery query, IBasicPage paging, out int count);
    }
}