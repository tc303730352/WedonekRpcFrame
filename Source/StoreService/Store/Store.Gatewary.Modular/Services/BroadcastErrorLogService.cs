using WeDonekRpc.Model;
using RpcStore.RemoteModel.BroadcastErrorLog;
using RpcStore.RemoteModel.BroadcastErrorLog.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class BroadcastErrorLogService : IBroadcastErrorLogService
    {
        public BroadcastLog GetBroadcastErrorLog(long id)
        {
            return new GetBroadcastErrorLog
            {
                Id = id,
            }.Send();
        }

        public BroadcastLogDatum[] QueryBroadcastLog(BroadcastErrorQuery query, string lang, IBasicPage paging, out int count)
        {
            return new QueryBroadcastLog
            {
                Query = query,
                Lang = lang,
                Index = paging.Index,
                Size = paging.Size,
                NextId = paging.NextId,
                SortName = paging.SortName,
                IsDesc = paging.IsDesc
            }.Send(out count);
        }

    }
}
