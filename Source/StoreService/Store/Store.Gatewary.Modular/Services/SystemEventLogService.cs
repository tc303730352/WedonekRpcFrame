using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.SysEventLog;
using RpcStore.RemoteModel.SysEventLog.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class SystemEventLogService : ISystemEventLogService
    {
        public SysEventLogData Get (long id)
        {
            return new GetSysEventLog
            {
                Id = id
            }.Send();
        }

        public PagingResult<SystemEventLogDto> Query (SysEventLogQuery query, IBasicPage paging)
        {
            return new QuerySysEventLog
            {
                Query = query,
                Index = paging.Index,
                Size = paging.Size,
                SortName = paging.SortName,
                IsDesc = paging.IsDesc,
                NextId = paging.NextId
            }.Send();
        }
    }
}
