using AutoTask.Collect.Model;
using AutoTask.Model.TaskItem;
using WeDonekRpc.Helper;
using RpcTaskModel.TaskPlan.Model;
using AutoTask.Service.Model;

namespace AutoTask.Service.Helper
{
    internal static class LinqHelper
    {
        public static TaskExecLog[] ExecTask (this TaskItemDto item, long rpcMerId, int? regionId)
        {
            return TaskItemHelper.ExecTask(item, regionId, rpcMerId);
        }
        public static DateTime GetExecTime (this TaskPlanBasic[] plans, DateTime? execTime)
        {
            if (plans.Length == 1)
            {
                return PlanHepler.GetExecTime(plans[0], execTime);
            }
            DateTime minTime = DateTime.MaxValue;
            DateTime now = DateTime.Now;
            plans.ForEach(c =>
            {
                DateTime time = PlanHepler.GetExecTime(c, execTime);
                if (time < minTime)
                {
                    minTime = time;
                }
            });
            return minTime;
        }
        public static DateTime GetNextTime (this TaskPlanBasic[] plans, DateTime execTime)
        {
            if (plans.Length == 1)
            {
                return PlanHepler.GetNextTime(plans[0], execTime);
            }
            DateTime minTime = DateTime.MaxValue;
            DateTime now = DateTime.Now;
            plans.ForEach(c =>
            {
                DateTime time = PlanHepler.GetNextTime(c, execTime);
                if (time < minTime)
                {
                    minTime = time;
                }
            });
            return minTime;
        }
    }
}
