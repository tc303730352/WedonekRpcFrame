using AutoTask.Collect.LocalEvent;
using AutoTask.DAL;
using AutoTask.Model;
using AutoTask.Model.DB;
using AutoTask.Model.Task;
using RpcTaskModel;
using RpcTaskModel.AutoTask.Model;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Error;
using WeDonekRpc.Model;

namespace AutoTask.Collect.Collect
{
    internal class AutoTaskCollect : IAutoTaskCollect
    {
        private readonly IAutoTaskDAL _BasicDAL;
        private readonly ICacheController _Cache;
        public AutoTaskCollect (IAutoTaskDAL dal, ICacheController cache)
        {
            this._Cache = cache;
            this._BasicDAL = dal;
        }
        public AutoTaskDatum[] Query (TaskQueryParam query, IBasicPage paging, out int count)
        {
            return this._BasicDAL.Query(query, paging, out count);
        }
        private void _Refresh (AutoTaskModel task, string evName)
        {
            new AutoTaskEvent
            {
                TaskId = task.Id,
                Task = task
            }.AsyncSend(evName);
        }
        private void _Refresh (long taskId, string evName)
        {
            new AutoTaskEvent
            {
                TaskId = taskId
            }.AsyncSend(evName);
        }
        private void _Refresh (AutoTaskModel task, AutoTaskStatus newStatus)
        {
            new AutoTaskEvent
            {
                TaskId = task.Id,
                Task = task,
                NewStatus = newStatus
            }.AsyncSend("status");
        }
        public void Refresh (long taskId)
        {
            new AutoTaskEvent
            {
                TaskId = taskId
            }.Send("refresh");
        }
        public long Add (AutoTaskAdd task)
        {
            if (this._BasicDAL.CheckIsRepeat(task.RpcMerId, task.TaskName))
            {
                throw new ErrorException("task.name.repeat");
            }
            AutoTaskModel add = task.ConvertMap<AutoTaskAdd, AutoTaskModel>();
            add.VerNum = Tools.GetRandom();
            add.TaskStatus = AutoTaskStatus.起草;
            add.Id = this._BasicDAL.Add(add);
            this._Refresh(add, "add");
            return add.Id;
        }
        public bool CheckTaskStatus (long taskId, AutoTaskStatus status)
        {
            AutoTaskStatus source = this._BasicDAL.GetTaskStatus(taskId);
            if (source == AutoTaskStatus.未知)
            {
                throw new ErrorException("task.not.find");
            }
            return source == status;
        }
        public AutoTaskModel Get (long id)
        {
            AutoTaskModel task = this._BasicDAL.Get(id);
            if (task == null)
            {
                throw new ErrorException("task.not.find");
            }
            return task;
        }
        public bool Set (AutoTaskModel task, AutoTaskSet set)
        {
            if (set.IsEquals(task))
            {
                return false;
            }
            else if (task.TaskName != set.TaskName && this._BasicDAL.CheckIsRepeat(task.RpcMerId, set.TaskName))
            {
                throw new ErrorException("task.name.repeat");
            }
            this._BasicDAL.SetAutoTask(task.Id, set);
            this._Refresh(task, "set");
            return true;
        }
        public bool SetTaskStatus (AutoTaskModel task, AutoTaskStatus status)
        {
            if (task.TaskStatus == status)
            {
                return false;
            }
            this._BasicDAL.SetTaskStatus(task.Id, status);
            this._Refresh(task, status);
            return true;
        }

        public void Delete (AutoTaskModel task)
        {
            this._BasicDAL.Delete(task.Id);
            this._Refresh(task, "delete");
        }
        public BasicTask[] GetTasks (long rpcMerId)
        {
            return this._BasicDAL.GetTaskList(rpcMerId);
        }
        public RemoteTask GetTask (long id, int verNum)
        {
            string key = string.Join("_", "Task", id, verNum);
            if (this._Cache.TryGet(key, out RemoteTask task))
            {
                return task;
            }
            task = this._BasicDAL.GetTask(id);
            _ = this._Cache.Set(key, task, new TimeSpan(1, 0, 0, 0, 0));
            return task;
        }
        public TaskState GetTaskState (long id, bool isRefresh)
        {
            if (isRefresh)
            {
                this.Refresh(id);
            }
            string key = string.Concat("TaskState_", id);
            if (this._Cache.TryGet(key, out TaskState state))
            {
                return state;
            }
            state = this._BasicDAL.GetTaskState(id);
            if (state == null)
            {
                throw new ErrorException("task.not.find");
            }
            _ = this._Cache.Add(key, state, new TimeSpan(10, 0, 0, 0, 0));
            return state;
        }

        public void StopTask (long id, string error)
        {
            long errorId = LocalErrorManage.GetErrorId(error);
            if (this._BasicDAL.StopTask(id, errorId))
            {
                this._Refresh(id, "stopTask");
            }
        }



        public void SetTaskTime (long id, DateTime execTime)
        {
            if (this._BasicDAL.SetTaskTime(id, execTime))
            {
                this._Refresh(id, "setTaskTime");
            }
        }

        public bool BeginExec (long id, DateTime next, int verNum, out int ver)
        {
            ver = Tools.GetRandom();
            if (this._BasicDAL.BeginExec(id, next, verNum, ver))
            {
                this._Refresh(id, "execBegin");
                return true;
            }
            return false;
        }

        public void ExecEnd (long id)
        {
            this._BasicDAL.EndExec(id);
            this._Refresh(id, "execEnd");
        }
    }
}
