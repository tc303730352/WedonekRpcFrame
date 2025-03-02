using WeDonekRpc.Model;
using RpcStore.RemoteModel.SysLog.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface ISysLogService
    {
        /// <summary>
        /// 获取系统日志详细
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <returns>系统日志数据</returns>
        SystemLogData GetSysLog(long id);

        /// <summary>
        /// 查询日志
        /// </summary>
        /// <param name="query"></param>
        /// <param name="paging">分页参数</param>
        /// <param name="count">数据总数</param>
        /// <returns></returns>
        SystemLog[] QuerySysLog(SysLogQuery query, IBasicPage paging, out int count);

    }
}
