using AutoTask.Model;
using AutoTask.Model.DB;
using AutoTask.Model.Task;
using WeDonekRpc.Model;
using RpcTaskModel;
using RpcTaskModel.AutoTask.Model;

namespace AutoTask.DAL
{
    public interface IAutoTaskDAL
    {
        AutoTaskDatum[] Query (TaskQueryParam query, IBasicPage paging, out int count);
        void Delete (long taskId);
        AutoTaskModel Get (long id);
        void SetAutoTask (long id, AutoTaskSet param);

        long Add (AutoTaskModel task);
        bool BeginExec (long id, DateTime next, int verNum, int ver);
        void EndExec (long id);
        RemoteTask GetTask (long id);
        BasicTask[] GetTaskList (long rpcMerId);
        TaskState GetTaskState (long id);
        bool SetTaskTime (long id, DateTime execTime);
        bool StopTask (long id, long errorId);
        bool CheckIsRepeat (long regionId, string taskName);
        AutoTaskStatus GetTaskStatus (long taskId);
        void SetTaskStatus (long id, AutoTaskStatus status);
    }
}