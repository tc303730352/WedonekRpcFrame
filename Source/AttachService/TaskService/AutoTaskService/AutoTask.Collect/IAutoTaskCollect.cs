using AutoTask.Model;
using AutoTask.Model.DB;
using AutoTask.Model.Task;
using RpcTaskModel;
using RpcTaskModel.AutoTask.Model;
using WeDonekRpc.Model;

namespace AutoTask.Collect
{
    public interface IAutoTaskCollect
    {
        void Refresh (long taskId);
        AutoTaskDatum[] Query (TaskQueryParam query, IBasicPage paging, out int count);
        long Add (AutoTaskAdd task);
        bool SetTaskStatus (AutoTaskModel task, AutoTaskStatus status);
        bool CheckTaskStatus (long taskId, AutoTaskStatus status);
        AutoTaskModel Get (long id);
        void Delete (AutoTaskModel task);

        bool BeginExec (long id, DateTime next, int verNum, out int ver);
        void ExecEnd (long id);
        RemoteTask GetTask (long id, int verNum);
        BasicTask[] GetTasks (long rpcMerId);
        TaskState GetTaskState (long id, bool isRefresh);
        void SetTaskTime (long id, DateTime execTime);
        void StopTask (long id, string error);
        bool Set (AutoTaskModel task, AutoTaskSet datum);
    }
}