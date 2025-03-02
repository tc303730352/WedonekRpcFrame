using WeDonekRpc.Helper.Validate;
namespace Store.Gatewary.Modular.Model.Identity
{
    /// <summary>
    /// 设置身份标识启用状态 UI参数实体
    /// </summary>
    internal class UI_SetIdentityIsEnable
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        [NullValidate("rpc.store.identity.id.null")]
        public long Id { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

    }
}
