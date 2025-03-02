using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
namespace Store.Gatewary.Modular.Model.ServerConfig
{
    /// <summary>
    /// 设置服务节点状态 UI参数实体
    /// </summary>
    internal class UI_SetServiceState
    {
        /// <summary>
        /// 节点ID
        /// </summary>
        [NumValidate("rpc.store.serverconfig.serviceId.error", 1)]
        public long ServiceId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [EnumValidate("rpc.store.serverconfig.state.error", typeof(RpcServiceState))]
        public RpcServiceState State { get; set; }

    }
}
