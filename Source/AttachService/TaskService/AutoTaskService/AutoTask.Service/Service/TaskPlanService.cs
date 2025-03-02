using AutoTask.Collect;
using AutoTask.Model.DB;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using RpcTaskModel;
using RpcTaskModel.TaskPlan.Model;
namespace AutoTask.Service.Service
{
    internal class TaskPlanService : ITaskPlanService
    {
        private readonly ITaskPlanCollect _TaskPlan;
        private readonly IAutoTaskCollect _Task;

        public TaskPlanService (ITaskPlanCollect taskPlan, IAutoTaskCollect task)
        {
            this._TaskPlan = taskPlan;
            this._Task = task;
        }

        public long Add (TaskPlanAdd plan)
        {
            if (plan.IsEnable && this._Task.CheckTaskStatus(plan.TaskId, AutoTaskStatus.启用))
            {
                throw new ErrorException("task.already.enable");
            }
            return this._TaskPlan.Add(plan);
        }

        public void Delete (long planId)
        {
            AutoTaskPlanModel plan = this._TaskPlan.Get(planId);
            if (plan.IsEnable && this._Task.CheckTaskStatus(plan.TaskId, AutoTaskStatus.启用))
            {
                throw new ErrorException("task.already.enable");
            }
            this._TaskPlan.Delete(plan);
        }

        public TaskPlanDatum Get (long planId)
        {
            AutoTaskPlanModel plan = this._TaskPlan.Get(planId);
            return plan.ConvertMap<AutoTaskPlanModel, TaskPlanDatum>();
        }

        public TaskPlanDatum[] Gets (long taskId)
        {
            AutoTaskPlanModel[] plans = this._TaskPlan.Gets(taskId);
            TaskPlanDatum[] tasks = plans.ConvertMap<AutoTaskPlanModel, TaskPlanDatum>();
            tasks.ForEach(c =>
            {
                if (c.PlanShow.IsNull())
                {
                    c.PlanShow = c.GetPlanShow();
                }
            });
            return tasks;
        }



        public bool Set (long planId, TaskPlanSet set)
        {
            AutoTaskPlanModel plan = this._TaskPlan.Get(planId);
            if (plan.IsEnable && this._Task.CheckTaskStatus(plan.TaskId, AutoTaskStatus.启用))
            {
                throw new ErrorException("task.already.enable");
            }
            return this._TaskPlan.Set(plan, set);
        }

        public bool SetIsEnable (long planId, bool isEnable)
        {
            AutoTaskPlanModel plan = this._TaskPlan.Get(planId);
            return this._TaskPlan.SetIsEnable(plan, isEnable);
        }

    }
}
