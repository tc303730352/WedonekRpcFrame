using AutoTask.Gateway.Interface;
using RpcTaskModel.TaskLog.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.HttpApiGateway.Model;

namespace AutoTask.Gateway.Api
{
    /// <summary>
    /// 任务日志
    /// </summary>
    internal class AutoTaskLogApi : ApiController
    {
        private readonly ITaskLogService _Service;

        public AutoTaskLogApi (ITaskLogService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 获取日志详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TaskLogDetailed Get ([NumValidate("rpc.store.task.log.id.error", 1)] long id)
        {
            return this._Service.Get(id);
        }
        /// <summary>
        /// 查询日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PagingResult<TaskLogDatum> Query (PagingParam<TaskLogQueryParam> param)
        {
            return this._Service.Query(param.Query, param.ToBasicPaging());
        }
    }
}
