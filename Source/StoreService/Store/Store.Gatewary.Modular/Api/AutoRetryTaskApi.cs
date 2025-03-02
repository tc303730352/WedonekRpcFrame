using RpcStore.RemoteModel.RetryTask.Model;
using Store.Gatewary.Modular.Interface;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;
using WeDonekRpc.HttpApiGateway.Model;
namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 自动重试服务
    /// </summary>
    internal class AutoRetryTaskApi : ApiController
    {
        private readonly IAutoRetryTaskService _Service;

        public AutoRetryTaskApi (IAutoRetryTaskService service)
        {
            this._Service = service;
        }
        [ApiPrower(false)]
        public void Add ()
        {
            this._Service.Add();
        }
        /// <summary>
        /// 取消任务
        /// </summary>
        /// <param name="ids">任务ID</param>
        public void Cancel ([NullValidate("rpc.store.retry.task.id.null")] long[] ids)
        {
            this._Service.Cancel(ids);
        }
        /// <summary>
        /// 获取任务详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RetryTaskDetailed Get ([NumValidate("rpc.store.retry.task.id.error", 1)] long id)
        {
            return this._Service.Get(id);
        }
        /// <summary>
        /// 查询重试任务日志
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public PagingResult<RetryTaskDatum> Query (PagingParam<RetryTaskQuery> query)
        {
            return this._Service.Query(query.Query, query.ToBasicPaging());
        }
        /// <summary>
        /// 重置任务
        /// </summary>
        /// <param name="obj"></param>
        public void Reset (LongParam<string> obj)
        {
            this._Service.Reset(obj.Id, obj.Value);
        }
    }
}
