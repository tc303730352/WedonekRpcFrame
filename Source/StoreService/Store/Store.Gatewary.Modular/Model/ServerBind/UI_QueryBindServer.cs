using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.ServerBind.Model;
namespace Store.Gatewary.Modular.Model.ServerBind
{
    /// <summary>
    /// 查询集群绑定的服务节点 UI参数实体
    /// </summary>
    internal class UI_QueryBindServer : BasicPage
    {
        /// <summary>
        /// 集群ID
        /// </summary>
        [NumValidate("rpc.store.serverbind.rpcMerId.error", 1)]
        public long RpcMerId { get; set; }

        /// <summary>
        /// 查询参数
        /// </summary>
        [NullValidate("rpc.store.serverbind.query.null")]
        public BindQueryParam Query { get; set; }

    }
}
