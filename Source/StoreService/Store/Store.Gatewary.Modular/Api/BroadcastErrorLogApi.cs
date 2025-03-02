using RpcStore.RemoteModel.BroadcastErrorLog.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model.BroadcastErrorLog;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 广播错误日志
    /// </summary>
    internal class BroadcastErrorLogApi : ApiController
    {
        private readonly IBroadcastErrorLogService _Service;
        public BroadcastErrorLogApi (IBroadcastErrorLogService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 获取广播错误日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns>广播错误日志</returns>
        public BroadcastLog Get ([NumValidate("rpc.store.broadcasterrorlog.id.error", 1)] long id)
        {
            return this._Service.GetBroadcastErrorLog(id);
        }

        /// <summary>
        /// 查询广播错误日志
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public PagingResult<BroadcastLogDatum> QueryBroadcastLog ([NullValidate("rpc.store.broadcasterrorlog.param.null")] UI_QueryBroadcastLog param)
        {
            BroadcastLogDatum[] results = this._Service.QueryBroadcastLog(param.Query, param.Lang, param, out int count);
            return new PagingResult<BroadcastLogDatum>(count, results);
        }

    }
}
