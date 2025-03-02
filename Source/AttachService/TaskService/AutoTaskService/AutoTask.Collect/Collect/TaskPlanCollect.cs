using AutoTask.DAL;
using AutoTask.Model.DB;
using AutoTask.Model.TaskPlan;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using RpcTaskModel.TaskPlan.Model;

namespace AutoTask.Collect.Collect
{
    internal class TaskPlanCollect : ITaskPlanCollect
    {
        private readonly IAutoTaskPlanDAL _BasicDAL;
        private readonly ICacheController _Cache;
        public TaskPlanCollect (IAutoTaskPlanDAL dal, ICacheController cache)
        {
            this._Cache = cache;
            this._BasicDAL = dal;
        }
        public void CheckTaskPlan (long taskId)
        {
            int num = this._BasicDAL.GetEnableCount(taskId);
            if (num == 0)
            {
                throw new ErrorException("task.plan.no.enable");
            }
        }
        public long Add (TaskPlanAdd plan)
        {
            AutoTaskPlanModel add = plan.ConvertMap<TaskPlanAdd, AutoTaskPlanModel>();
            add.PlanOnlyNo = plan.GetPlanOnlyNo();
            if (this._BasicDAL.CheckIsRepeat(plan.TaskId, add.PlanOnlyNo))
            {
                throw new ErrorException("task.plan.repeat");
            }
            add.PlanShow = plan.GetPlanShow();
            return this._BasicDAL.Add(add);
        }


        public void Delete (AutoTaskPlanModel plan)
        {
            this._BasicDAL.Delete(plan.Id);
        }

        public AutoTaskPlanModel Get (long id)
        {
            AutoTaskPlanModel plan = this._BasicDAL.Get(id);
            if (plan == null)
            {
                throw new ErrorException("task.plan.not.find");
            }
            return plan;
        }

        public AutoTaskPlanModel[] Gets (long taskId)
        {
            return this._BasicDAL.Gets(taskId);
        }

        public TaskPlanBasic[] GetTaskPlans (long taskId, int verNum)
        {
            string key = string.Join("_", "TaskPlan", taskId, verNum);
            if (this._Cache.TryGet(key, out TaskPlanBasic[] plan))
            {
                return plan;
            }
            plan = this._BasicDAL.GetTaskPlans(taskId);
            _ = this._Cache.Set(key, plan, new TimeSpan(1, 0, 0, 0));
            return plan;
        }


        public bool Set (AutoTaskPlanModel plan, TaskPlanSet set)
        {
            if (set.IsEquals(plan))
            {
                return false;
            }
            string onlyNo = set.GetPlanOnlyNo();
            if (onlyNo != plan.PlanOnlyNo && this._BasicDAL.CheckIsRepeat(plan.TaskId, onlyNo))
            {
                throw new ErrorException("task.plan.repeat");
            }
            TaskPlanSetParam param = set.ConvertMap<TaskPlanSet, TaskPlanSetParam>();
            param.PlanOnlyNo = onlyNo;
            param.PlanShow = set.GetPlanShow();
            this._BasicDAL.Set(plan.Id, param);
            return true;
        }

        public bool SetIsEnable (AutoTaskPlanModel plan, bool isEnable)
        {
            if (plan.IsEnable == isEnable)
            {
                return false;
            }
            this._BasicDAL.SetIsEnable(plan.Id, isEnable);
            return true;
        }

    }
}
