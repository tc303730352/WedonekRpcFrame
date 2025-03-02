using WeDonekRpc.Client.Interface;
using RpcTaskModel.TaskPlan;
using RpcTaskModel.TaskPlan.Model;

namespace AutoTask.Service.Event
{
    internal class TaskPlanEvent : IRpcApiService
    {
        private readonly ITaskPlanService _Service;

        public TaskPlanEvent (ITaskPlanService service)
        {
            this._Service = service;
        }


        public long AddTaskPlan (AddTaskPlan add)
        {
            return this._Service.Add(add.TaskPlan);
        }

        public void DeleteTaskPlan (DeleteTaskPlan obj)
        {
            this._Service.Delete(obj.Id);
        }

        public TaskPlanDatum GetTaskPlan (GetTaskPlan obj)
        {
            return this._Service.Get(obj.Id);
        }

        public TaskPlanDatum[] GetTaskPlans (GetTaskPlans obj)
        {
            return this._Service.Gets(obj.TaskId);
        }

        public bool SetTaskPlan (SetTaskPlan set)
        {
            return this._Service.Set(set.Id, set.TaskPlan);
        }

        public bool SetTaskPlanIsEnable (SetTaskPlanIsEnable obj)
        {
            return this._Service.SetIsEnable(obj.Id, obj.IsEnable);
        }
    }
}
