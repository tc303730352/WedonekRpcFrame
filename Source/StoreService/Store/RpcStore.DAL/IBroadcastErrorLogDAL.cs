using RpcStore.Model.BroadcastErrorLog;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.BroadcastErrorLog.Model;
using WeDonekRpc.Model;

namespace RpcStore.DAL
{
    public interface IBroadcastErrorLogDAL
    {
        BroadcastErrorLogModel Get (long id);
        BroadcastErrorLog[] Query (BroadcastErrorQuery query, IBasicPage paging, out int count);
    }
}