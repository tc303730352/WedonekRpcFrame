using WeDonekRpc.Helper.Validate;
namespace Store.Gatewary.Modular.Model.ResourceModular
{
    /// <summary>
    /// 设置资源模块备注信息 UI参数实体
    /// </summary>
    internal class UI_SetModularRemark
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        [NumValidate("rpc.store.resourcemodular.id.error", 1)]
        public long Id { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [NullValidate("rpc.store.resourcemodular.remark.null")]
        public string Remark { get; set; }

    }
}
