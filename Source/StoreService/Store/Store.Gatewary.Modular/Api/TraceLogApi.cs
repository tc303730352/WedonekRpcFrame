using RpcStore.RemoteModel.TraceLog.Model;
using Store.Gatewary.Modular.Interface;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 链路日志查询(明细)
    /// </summary>
    internal class TraceLogApi : ApiController
    {
        private readonly ITraceLogService _Service;
        public TraceLogApi (ITraceLogService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 获取单个链路下的日志
        /// </summary>
        /// <param name="traceId">链路ID</param>
        /// <returns>链路日志</returns>
        public TraceLog[] GetTraceByTraceId ([NullValidate("rpc.store.tracelog.traceId.null")] string traceId)
        {
            return this._Service.GetTraceByTraceId(traceId);
        }

        /// <summary>
        /// 获取单个链路日志详细
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <returns>链路日志</returns>
        public TraceLogDatum Get ([NumValidate("rpc.store.tracelog.id.error", 1)] long id)
        {
            return this._Service.GetTraceLog(id);
        }

    }
}
