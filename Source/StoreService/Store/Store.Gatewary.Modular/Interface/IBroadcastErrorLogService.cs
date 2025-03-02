using WeDonekRpc.Model;
using RpcStore.RemoteModel.BroadcastErrorLog.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IBroadcastErrorLogService
    {
        /// <summary>
        /// 获取广播错误日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns>广播错误日志</returns>
        BroadcastLog GetBroadcastErrorLog(long id);

        /// <summary>
        /// 查询广播错误日志
        /// </summary>
        /// <param name="query">查询参数</param>
        /// <param name="lang">返回错误语言类型</param>
        /// <param name="paging">分页参数</param>
        /// <param name="count">数据总数</param>
        /// <returns></returns>
        BroadcastLogDatum[] QueryBroadcastLog(BroadcastErrorQuery query, string lang, IBasicPage paging, out int count);

    }
}
