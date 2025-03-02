using AutoTask.Model.DB;
using RpcTaskModel.TaskPlan.Model;

namespace AutoTask.Collect
{
    public interface ITaskPlanCollect
    {
        void CheckTaskPlan (long taskId);
        AutoTaskPlanModel[] Gets (long taskId);
        bool Set (AutoTaskPlanModel plan, TaskPlanSet set);
        AutoTaskPlanModel Get (long id);
        void Delete (AutoTaskPlanModel plan);
        long Add (TaskPlanAdd plan);
        bool SetIsEnable (AutoTaskPlanModel plan, bool isEnable);
        TaskPlanBasic[] GetTaskPlans (long taskId, int verNum);
    }
}