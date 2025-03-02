using AutoTask.Gateway.Interface;
using AutoTask.Gateway.Model;
using RpcTaskModel.TaskItem.Model;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.HttpApiGateway.Model;

namespace AutoTask.Gateway.Api
{
    /// <summary>
    /// 任务项
    /// </summary>
    internal class TaskItemApi : ApiController
    {
        private readonly ITaskItemService _Service;

        public TaskItemApi (ITaskItemService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 新增任务项
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public long Add (LongParam<TaskItemSetParam> param)
        {
            return this._Service.Add(param.Id, param.Value);
        }
        /// <summary>
        /// 删除任务项
        /// </summary>
        /// <param name="id"></param>
        public void Delete ([NumValidate("rpc.store.task.item.id.error", 1)] long id)
        {
            this._Service.Delete(id);
        }
        /// <summary>
        /// 获取任务项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TaskItemDatum Get ([NumValidate("rpc.store.task.plan.id.error", 1)] long id)
        {
            return this._Service.Get(id);
        }
        /// <summary>
        /// 获取选择的任务项
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public TaskSelectItem[] GetSelectItems ([NumValidate("rpc.store.task.id.error", 1)] long taskId)
        {
            return this._Service.GetSelectItems(taskId);
        }
        /// <summary>
        /// 获取任务详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AutoTaskItem GetDetailed ([NumValidate("rpc.store.task.plan.id.error", 1)] long id)
        {
            return this._Service.GetDetailed(id);
        }
        /// <summary>
        /// 获取任务项列表
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public TaskItem[] Gets ([NumValidate("rpc.store.task.id.error", 1)] long taskId)
        {
            return this._Service.Gets(taskId);
        }
        public void GetSelect ()
        {

        }
        /// <summary>
        /// 修改任务项
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool Set (LongParam<TaskItemSetParam> param)
        {
            return this._Service.Set(param.Id, param.Value);
        }
    }
}
