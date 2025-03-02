using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.SysLog.Model;

namespace RpcStore.Service.Interface
{
    public interface ISysLogService
    {
        SystemLogData GetSysLog(long id);
        PagingResult<SystemLog> Query(SysLogQuery query, IBasicPage paging);
    }
}