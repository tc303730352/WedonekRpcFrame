using WeDonekRpc.Model.Model;
using RpcStore.RemoteModel.LimitConfig.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface ILimitConfigService
    {
        /// <summary>
        /// 删除服务节点全局限流配置
        /// </summary>
        /// <param name="serverId">服务节点ID</param>
        void DeleteLimitConfig(long serverId);

        /// <summary>
        /// 获取服务节点全局限流配置
        /// </summary>
        /// <param name="serverId">服务节点ID</param>
        /// <returns></returns>
        ServerLimitConfig GetLimitConfig(long serverId);

        /// <summary>
        /// 同步服务节点全局限流配置(添加或修改)
        /// </summary>
        /// <param name="config">全局限流配置</param>
        void SyncLimitConfig(LimitConfigDatum config);

    }
}
