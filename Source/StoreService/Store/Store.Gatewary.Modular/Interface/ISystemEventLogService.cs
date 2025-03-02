using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.SysEventLog.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface ISystemEventLogService
    {
        SysEventLogData Get (long id);
        PagingResult<SystemEventLogDto> Query (SysEventLogQuery query, IBasicPage paging);
    }
}