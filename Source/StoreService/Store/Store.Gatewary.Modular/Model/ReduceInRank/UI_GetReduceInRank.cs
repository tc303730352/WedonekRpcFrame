using WeDonekRpc.Helper.Validate;
namespace Store.Gatewary.Modular.Model.ReduceInRank
{
    /// <summary>
    /// 获取集群下某个服务节点降级配置 UI参数实体
    /// </summary>
    internal class UI_GetReduceInRank
    {
        /// <summary>
        /// 集群ID
        /// </summary>
        [NumValidate("rpc.store.reduceinrank.rpcMerId.error", 1)]
        public long RpcMerId { get; set; }

        /// <summary>
        /// 服务节点ID
        /// </summary>
        [NumValidate("rpc.store.reduceinrank.serverId.error", 1)]
        public long ServerId { get; set; }

    }
}
