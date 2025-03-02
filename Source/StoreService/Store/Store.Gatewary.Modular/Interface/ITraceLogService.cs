using RpcStore.RemoteModel.TraceLog.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface ITraceLogService
    {
        /// <summary>
        /// 获取单个链路下的日志
        /// </summary>
        /// <param name="traceId">链路ID</param>
        /// <returns>链路日志</returns>
        TraceLog[] GetTraceByTraceId(string traceId);

        /// <summary>
        /// 获取单个链路日志详细
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <returns>链路日志</returns>
        TraceLogDatum GetTraceLog(long id);

    }
}
