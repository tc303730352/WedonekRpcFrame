using WeDonekRpc.Helper.Validate;
namespace Store.Gatewary.Modular.Model.DictateNode
{
    /// <summary>
    /// 设置广播指令节点路由名称 UI参数实体
    /// </summary>
    internal class UI_SetDictateName
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        [NumValidate("rpc.store.dictatenode.id.error", 1)]
        public long Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [NullValidate("rpc.store.dictatenode.name.null")]
        public string Name { get; set; }

    }
}
