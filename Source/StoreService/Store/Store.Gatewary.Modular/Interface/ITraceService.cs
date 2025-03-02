using WeDonekRpc.Model;
using RpcStore.RemoteModel.Trace.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface ITraceService
    {
        /// <summary>
        /// 查询链路信息
        /// </summary>
        /// <param name="query">查询参数</param>
        /// <param name="paging">分页参数</param>
        /// <param name="count">数据总数</param>
        /// <returns></returns>
        RpcTrace[] QueryTrace(TraceQuery query, IBasicPage paging, out int count);

    }
}
