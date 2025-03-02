using RpcStore.RemoteModel.ReduceInRank.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IReduceInRankService
    {
        /// <summary>
        /// 删除服务节点降级配置
        /// </summary>
        /// <param name="id">数据ID</param>
        void DeleteReduceInRank(long id);

        /// <summary>
        /// 获取集群下某个服务节点降级配置
        /// </summary>
        /// <param name="rpcMerId">集群ID</param>
        /// <param name="serverId">服务节点ID</param>
        /// <returns></returns>
        ReduceInRankConfig GetReduceInRank(long rpcMerId, long serverId);

        /// <summary>
        /// 服务节点降级配置
        /// </summary>
        /// <param name="datum">降级配置</param>
        void SyncReduceInRank(ReduceInRankAdd datum);

    }
}
