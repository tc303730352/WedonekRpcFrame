namespace RpcTaskModel.TaskPlan.Model
{
    /// <summary>
    /// 任务计划
    /// </summary>
    public class TaskPlanDatum : TaskPlanSet
    {
        /// <summary>
        /// 计划ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 计划说明
        /// </summary>
        public string PlanShow { get; set; }
    }
}
