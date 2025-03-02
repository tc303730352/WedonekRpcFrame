using AutoTask.Gateway.Interface;
using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.HttpApiGateway.Model;
using WeDonekRpc.Helper.Validate;
using RpcTaskModel.TaskPlan.Model;

namespace AutoTask.Gateway.Api
{
    /// <summary>
    /// 任务计划
    /// </summary>
    internal class TaskPlanApi : ApiController
    {
        private readonly ITaskPlanService _Service;

        public TaskPlanApi (ITaskPlanService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 添加任务计划
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        public long Add (TaskPlanAdd add)
        {
            return this._Service.Add(add);
        }
        /// <summary>
        /// 删除任务计划
        /// </summary>
        /// <param name="id"></param>
        public void Delete ([NumValidate("rpc.store.task.plan.id.error", 1)] long id)
        {
            this._Service.Delete(id);
        }
        /// <summary>
        /// 获取任务计划
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TaskPlanDatum Get ([NumValidate("rpc.store.task.plan.id.error", 1)] long id)
        {
            return this._Service.Get(id);
        }
        /// <summary>
        /// 获取任务计划列表
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public TaskPlanDatum[] Gets ([NumValidate("rpc.store.task.id.error", 1)] long taskId)
        {
            return this._Service.Gets(taskId);
        }
        /// <summary>
        /// 修改任务计划
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public bool Set (LongParam<TaskPlanSet> set)
        {
            return this._Service.Set(set.Id, set.Value);
        }
        /// <summary>
        /// 设置任务计划状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool SetIsEnable (LongParam<bool> set)
        {
            return this._Service.SetIsEnable(set.Id, set.Value);
        }
    }
}
