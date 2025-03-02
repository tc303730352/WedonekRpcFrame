using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.ServerConfig.Model;
namespace Store.Gatewary.Modular.Model.ServerConfig
{
    /// <summary>
    /// 查询服务节点 UI参数实体
    /// </summary>
    internal class UI_QueryServer : BasicPage
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        [NullValidate("rpc.store.serverconfig.query.null")]
        public ServerConfigQuery Query { get; set; }

    }
}
