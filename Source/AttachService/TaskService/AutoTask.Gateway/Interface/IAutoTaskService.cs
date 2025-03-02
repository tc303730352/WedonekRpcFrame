using AutoTask.Gateway.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcTaskModel.AutoTask.Model;

namespace AutoTask.Gateway.Interface
{
    public interface IAutoTaskService
    {
        long Add (AutoTaskAdd add);
        void Delete (long id);
        bool Disable (long id, bool isEndTask);
        bool Enable (long id);
        AutoTaskInfo Get (long id);
        AutoTaskData GetDatum (long id);
        PagingResult<AutoTaskDatum> Query (TaskQueryParam query, IBasicPage paging);
        bool Set (long id, AutoTaskSet set);
    }
}