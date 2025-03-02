using WeDonekRpc.Helper.Validate;
namespace Store.Gatewary.Modular.Model.ClientLimit
{
    /// <summary>
    /// 获取服务节点限流配置 UI参数实体
    /// </summary>
    internal class UI_GetClientLimit
    {
        /// <summary>
        /// 集群ID
        /// </summary>
        [NumValidate("rpc.store.clientlimit.rpcMerId.error", 1)]
        public long RpcMerId { get; set; }

        /// <summary>
        /// 服务节点ID
        /// </summary>
        [NumValidate("rpc.store.clientlimit.serverId.error", 1)]
        public long ServerId { get; set; }

    }
}
