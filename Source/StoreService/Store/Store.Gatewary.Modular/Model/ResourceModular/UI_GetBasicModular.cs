using WeDonekRpc.Helper.Validate;
namespace Store.Gatewary.Modular.Model.ResourceModular
{
    /// <summary>
    /// 获取资源模块 UI参数实体
    /// </summary>
    internal class UI_GetBasicModular
    {
        /// <summary>
        /// 集群ID
        /// </summary>
        [NumValidate("rpc.store.resourcemodular.rpcMerId.error", 1)]
        public long RpcMerId { get; set; }

        /// <summary>
        /// 节点系统类型
        /// </summary>
        [NullValidate("rpc.store.resourcemodular.systemType.null")]
        public string SystemType { get; set; }

    }
}
