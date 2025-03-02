using AutoTask.Model;
using AutoTask.Model.DB;
using AutoTask.Model.Task;
using RpcTaskModel;
using RpcTaskModel.AutoTask.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model;
using WeDonekRpc.SqlSugar;

namespace AutoTask.DAL.Repository
{
    internal class AutoTaskDAL : IAutoTaskDAL
    {
        private readonly IRepository<AutoTaskModel> _BasicDAL;
        public AutoTaskDAL (IRepository<AutoTaskModel> dal)
        {
            this._BasicDAL = dal;
        }
        public void SetAutoTask (long id, AutoTaskSet param)
        {
            if (!this._BasicDAL.Update(param, a => a.Id == id))
            {
                throw new ErrorException("task.set.fail");
            }
        }
        public AutoTaskDatum[] Query (TaskQueryParam query, IBasicPage paging, out int count)
        {
            paging.InitOrderBy("Id", true);
            return this._BasicDAL.Query<AutoTaskDatum>(query.ToWhere(), paging, out count);
        }
        public long Add (AutoTaskModel task)
        {
            task.Id = IdentityHelper.CreateId();
            this._BasicDAL.Insert(task);
            return task.Id;
        }
        public void Delete (long taskId)
        {
            if (!this._BasicDAL.Delete(a => a.Id == taskId))
            {
                throw new ErrorException("task.delete.fail");
            }
        }
        public AutoTaskStatus GetTaskStatus (long taskId)
        {
            return this._BasicDAL.Get(c => c.Id == taskId, c => c.TaskStatus);
        }
        public AutoTaskModel Get (long id)
        {
            return this._BasicDAL.Get(a => a.Id == id);
        }
        public BasicTask[] GetTaskList (long rpcMerId)
        {
            return this._BasicDAL.Gets<BasicTask>(c => c.RpcMerId == rpcMerId && c.TaskStatus == AutoTaskStatus.启用);
        }
        public TaskState GetTaskState (long id)
        {
            return this._BasicDAL.Get<TaskState>(a => a.Id == id);
        }
        public RemoteTask GetTask (long id)
        {
            return this._BasicDAL.Get<RemoteTask>(a => a.Id == id);
        }


        public void EndExec (long id)
        {
            if (!this._BasicDAL.Update(new
            {
                LastExecEndTime = DateTime.Now,
                IsExec = false
            }, c => c.Id == id))
            {
                throw new ErrorException("task.end.fail");
            }
        }
        public bool BeginExec (long id, DateTime next, int verNum, int ver)
        {
            return this._BasicDAL.Update<object>(new
            {
                NextExecTime = next,
                LastExecTime = DateTime.Now,
                ExecVerNum = ver,
                IsExec = true
            }, c => c.Id == id && c.ExecVerNum == verNum);
        }

        public bool SetTaskTime (long id, DateTime execTime)
        {
            return this._BasicDAL.Update(a => a.NextExecTime == execTime, a => a.Id == id && a.TaskStatus == AutoTaskStatus.启用);
        }

        public bool CheckIsRepeat (long rpcMerId, string taskName)
        {
            return this._BasicDAL.IsExist(c => c.RpcMerId == rpcMerId && c.TaskName == taskName);
        }
        public bool StopTask (long id, long errorId)
        {
            return this._BasicDAL.Update(new
            {
                TaskStatus = AutoTaskStatus.错误,
                ErrorId = errorId,
                StopTime = DateTime.Now,
            }, a => a.Id == id && a.TaskStatus == AutoTaskStatus.启用);
        }

        public void SetTaskStatus (long id, AutoTaskStatus status)
        {
            int verNum = Tools.GetRandom();
            if (!this._BasicDAL.Update(new
            {
                TaskStatus = status,
                VerNum = verNum
            }, a => a.Id == id))
            {
                throw new ErrorException("task.set.fail");
            }
        }
    }
}
