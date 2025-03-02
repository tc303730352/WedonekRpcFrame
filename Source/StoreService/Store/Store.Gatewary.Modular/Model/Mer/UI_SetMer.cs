using WeDonekRpc.Helper.Validate;
using RpcStore.RemoteModel.Mer.Model;
namespace Store.Gatewary.Modular.Model.Mer
{
    /// <summary>
    /// 修改服务集群资料 UI参数实体
    /// </summary>
    internal class UI_SetMer
    {
        /// <summary>
        /// 集群ID
        /// </summary>
        [NumValidate("rpc.store.mer.rpcMerId.error", 1)]
        public long RpcMerId { get; set; }

        /// <summary>
        /// 集群资料
        /// </summary>
        [NullValidate("rpc.store.mer.datum.null")]
        public RpcMerSet Datum { get; set; }

    }
}
