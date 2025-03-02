using WeDonekRpc.Helper.Validate;
using RpcStore.RemoteModel.ServerConfig.Model;
namespace Store.Gatewary.Modular.Model.ServerConfig
{
    /// <summary>
    /// 修改服务节点资料 UI参数实体
    /// </summary>
    internal class UI_SetServer
    {
        /// <summary>
        /// 服务节点
        /// </summary>
        [NumValidate("rpc.store.serverconfig.serverId.error", 1)]
        public long ServerId { get; set; }

        /// <summary>
        /// 修改的资料
        /// </summary>
        [NullValidate("rpc.store.serverconfig.datum.null")]
        public ServerConfigSet Datum { get; set; }

    }
}
