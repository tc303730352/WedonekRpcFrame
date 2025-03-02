using RpcTaskModel.TaskLog.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace AutoTask.Gateway.Interface
{
    public interface ITaskLogService
    {
        TaskLogDetailed Get (long id);
        PagingResult<TaskLogDatum> Query (TaskLogQueryParam query, IBasicPage paging);
    }
}