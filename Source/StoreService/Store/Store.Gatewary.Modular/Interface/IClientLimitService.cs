using RpcStore.RemoteModel.ClientLimit.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IClientLimitService
    {
        ClientLimitModel[] GetAllClientLimit (long serverId);
        /// <summary>
        /// 删除服务节点限流配置
        /// </summary>
        /// <param name="id">数据ID</param>
        void DeleteClientLimit (long id);

        /// <summary>
        /// 获取服务节点限流配置
        /// </summary>
        /// <param name="rpcMerId">集群ID</param>
        /// <param name="serverId">服务节点ID</param>
        /// <returns>服务节点限流配置</returns>
        ClientLimitData GetClientLimit (long rpcMerId, long serverId);

        /// <summary>
        /// 添加或设置服务节点限流配置
        /// </summary>
        /// <param name="datum">限流配置</param>
        void SyncClientLimit (ClientLimitDatum datum);

    }
}
