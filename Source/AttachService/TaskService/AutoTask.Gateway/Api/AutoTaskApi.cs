using AutoTask.Gateway.Interface;
using AutoTask.Gateway.Model;
using RpcTaskModel.AutoTask.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.HttpApiGateway.Model;

namespace AutoTask.Gateway.Api
{
    /// <summary>
    /// 任务管理
    /// </summary>
    internal class AutoTaskApi : ApiController
    {
        private readonly IAutoTaskService _Service;

        public AutoTaskApi (IAutoTaskService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        public long Add (AutoTaskAdd add)
        {
            return this._Service.Add(add);
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="id"></param>
        public void Delete ([NumValidate("rpc.store.task.plan.id.error", 1)] long id)
        {
            this._Service.Delete(id);
        }
        /// <summary>
        /// 禁用任务
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public bool Disable (LongParam<bool> set)
        {
            return this._Service.Disable(set.Id, set.Value);
        }
        /// <summary>
        /// 启用任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Enable ([NumValidate("rpc.store.task.plan.id.error", 1)] long id)
        {
            return this._Service.Enable(id);
        }

        /// <summary>
        /// 获取任务信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AutoTaskInfo Get ([NumValidate("rpc.store.task.plan.id.error", 1)] long id)
        {
            return this._Service.Get(id);
        }
        /// <summary>
        /// 获取任务详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AutoTaskData GetDatum ([NumValidate("rpc.store.task.plan.id.error", 1)] long id)
        {
            return this._Service.GetDatum(id);
        }
        /// <summary>
        /// 查询任务列表
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public PagingResult<AutoTaskDatum> Query (PagingParam<TaskQueryParam> set)
        {
            return this._Service.Query(set.Query, set);
        }
        /// <summary>
        /// 修改任务信息
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public bool Set (LongParam<AutoTaskSet> set)
        {
            return this._Service.Set(set.Id, set.Value);
        }
    }
}
