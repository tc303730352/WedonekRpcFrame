using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.SysEventLog.Model;

namespace RpcStore.Service.Interface
{
    public interface ISystemEventLogService
    {
        SysEventLogData Get (long id);
        PagingResult<SystemEventLogDto> Query (SysEventLogQuery query, IBasicPage paging);
    }
}