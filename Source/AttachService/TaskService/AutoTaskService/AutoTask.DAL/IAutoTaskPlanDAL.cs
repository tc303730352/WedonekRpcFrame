using AutoTask.Model.DB;
using AutoTask.Model.TaskPlan;
using RpcTaskModel.TaskPlan.Model;

namespace AutoTask.DAL
{
    public interface IAutoTaskPlanDAL
    {
        bool CheckIsRepeat(long taskId, string planOnlyNo);
        void SetIsEnable(long id, bool isEnable);
        void Set(long id, TaskPlanSetParam set);
        long Add(AutoTaskPlanModel plan);
        AutoTaskPlanModel[] Gets(long taskId);
        AutoTaskPlanModel Get(long id);
        void Delete(long id);
        TaskPlanBasic[] GetTaskPlans(long taskId);
        int GetEnableCount(long taskId);
        void Clear(long taskId);
    }
}