using RpcStore.RemoteModel.Trace.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model.Trace;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 链路跟踪信息查询(主表）
    /// </summary>
    internal class TraceApi : ApiController
    {
        private readonly ITraceService _Service;
        public TraceApi (ITraceService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 查询链路信息
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public PagingResult<RpcTrace> Query ([NullValidate("rpc.store.trace.param.null")] UI_QueryTrace param)
        {
            RpcTrace[] results = this._Service.QueryTrace(param.Query, param, out int count);
            return new PagingResult<RpcTrace>(count, results);
        }

    }
}
