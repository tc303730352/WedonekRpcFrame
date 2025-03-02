using WeDonekRpc.Model;
using RpcStore.RemoteModel.DictateLimit.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IDictateLimitService
    {
        /// <summary>
        /// 添加服务节点指令限流配置
        /// </summary>
        /// <param name="datum">指令限流配置</param>
        /// <returns></returns>
        long AddDictateLimit(DictateLimitAdd datum);

        /// <summary>
        /// 删除指令限流配置
        /// </summary>
        /// <param name="id">数据ID</param>
        void DeleteDictateLimit(long id);

        /// <summary>
        /// 获取指令限流配置
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <returns>指令限流配置</returns>
        DictateLimit GetDictateLimit(long id);

        /// <summary>
        /// 查询指令限流配置
        /// </summary>
        /// <param name="query">查询参数</param>
        /// <param name="paging">分页参数</param>
        /// <param name="count">数据总数</param>
        /// <returns>指令限流配置</returns>
        DictateLimit[] QueryDictateLimit(DictateLimitQuery query, IBasicPage paging, out int count);

        /// <summary>
        /// 设置指令限流配置
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <param name="datum">指令限流资料</param>
        void SetDictateLimit(long id, DictateLimitSet datum);

    }
}
