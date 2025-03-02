using WeDonekRpc.Helper.Validate;
namespace RpcTaskModel.TaskPlan.Model
{
    /// <summary>
    /// 任务计划
    /// </summary>
    public class TaskPlanAdd : TaskPlanSet
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        [NumValidate("rpc.store.task.id.error", 1)]
        public long TaskId { get; set; }
    }
}
