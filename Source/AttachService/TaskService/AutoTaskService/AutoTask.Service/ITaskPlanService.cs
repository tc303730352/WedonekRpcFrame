using RpcTaskModel.TaskPlan.Model;

namespace AutoTask.Service
{
    public interface ITaskPlanService
    {
        long Add (TaskPlanAdd plan);
        void Delete (long planId);
        TaskPlanDatum Get (long planId);
        TaskPlanDatum[] Gets (long taskId);
        bool Set (long planId, TaskPlanSet set);
        bool SetIsEnable (long planId, bool isEnable);
    }
}