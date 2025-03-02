using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcTaskModel.AutoTask.Model;

namespace AutoTask.Service
{
    public interface IAutoTaskService
    {
        long Add(AutoTaskAdd task);
        void Delete(long taskId);
        bool EnableTask(long taskId);
        AutoTaskInfo Get(long taskId);
        PagingResult<AutoTaskBasic> Query(TaskQueryParam query, IBasicPage paging);
        bool Set(long taskId, AutoTaskSet datum);
        bool DisableTask(long taskId, bool isEndTask);
    }
}