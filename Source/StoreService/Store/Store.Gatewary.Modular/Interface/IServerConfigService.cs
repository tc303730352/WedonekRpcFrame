using WeDonekRpc.Model;
using RpcStore.RemoteModel.ServerConfig.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IServerConfigService
    {
        /// <summary>
        /// 新增服务节点
        /// </summary>
        /// <param name="datum">节点资料</param>
        /// <returns></returns>
        long AddServer (ServerConfigAdd datum);

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="serverId">服务节点ID</param>
        void DeleteServer (long serverId);
        ServerItem[] GetItems (ServerConfigQuery query);

        /// <summary>
        /// 获取服务节点资料
        /// </summary>
        /// <param name="serverId">服务节点ID</param>
        /// <returns></returns>
        RemoteServerDatum GetServer (long serverId);
        /// <summary>
        /// 获取服务节点资料
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        RemoteServerModel GetServerDatum (long serverId);
        /// <summary>
        /// 查询服务节点
        /// </summary>
        /// <param name="query">查询参数</param>
        /// <param name="paging">分页参数</param>
        /// <param name="count">数据总数</param>
        /// <returns></returns>
        RemoteServer[] QueryServer (ServerConfigQuery query, IBasicPage paging, out int count);

        /// <summary>
        /// 修改服务节点资料
        /// </summary>
        /// <param name="serverId">服务节点</param>
        /// <param name="datum">修改的资料</param>
        void SetServer (long serverId, ServerConfigSet datum);

        /// <summary>
        /// 设置服务节点状态
        /// </summary>
        /// <param name="serviceId">节点ID</param>
        /// <param name="state">状态</param>
        void SetServiceState (long serviceId, RpcServiceState state);
        void SetVerNum (long id, int verNum);
    }
}
