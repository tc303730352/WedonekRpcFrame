using RpcTaskModel.TaskPlan.Model;

namespace AutoTask.Gateway.Interface
{
    public interface ITaskPlanService
    {
        long Add (TaskPlanAdd add);
        void Delete (long id);
        TaskPlanDatum Get (long id);
        TaskPlanDatum[] Gets (long taskId);
        bool Set (long id, TaskPlanSet planSet);
        bool SetIsEnable (long id, bool isEnable);
    }
}