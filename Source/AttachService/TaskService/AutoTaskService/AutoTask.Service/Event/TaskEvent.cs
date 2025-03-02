using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcTaskModel.AutoTask;
using RpcTaskModel.AutoTask.Model;

namespace AutoTask.Service.Event
{
    internal class TaskEvent : IRpcApiService
    {
        private readonly IAutoTaskService _Task;
        public TaskEvent (IAutoTaskService task)
        {
            this._Task = task;
        }

        public long AddTask (AddTask add)
        {
            return this._Task.Add(add.Datum);
        }

        public void DeleteTask (DeleteTask obj)
        {
            this._Task.Delete(obj.TaskId);
        }

        public bool EnableTask (EnableTask obj)
        {
            return this._Task.EnableTask(obj.TaskId);
        }

        public PagingResult<AutoTaskBasic> QueryTask (QueryTask query)
        {
            return this._Task.Query(query.QueryParam, query.ToBasicPage());
        }
        public bool DisableTask (DisableTask obj)
        {
            return this._Task.DisableTask(obj.TaskId, obj.IsEndTask);
        }
        public AutoTaskInfo GetTask (GetTask obj)
        {
            return this._Task.Get(obj.TaskId);
        }
        public bool SetTask (SetTask set)
        {
            return this._Task.Set(set.Id, set.Datum);
        }
    }
}
