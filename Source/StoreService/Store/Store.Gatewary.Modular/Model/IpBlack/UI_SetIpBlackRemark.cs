using WeDonekRpc.Helper.Validate;
namespace Store.Gatewary.Modular.Model.IpBlack
{
    /// <summary>
    /// 修改Ip黑名单备注 UI参数实体
    /// </summary>
    internal class UI_SetIpBlackRemark
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        [NumValidate("rpc.store.ipblack.id.error", 1)]
        public long Id { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [NullValidate("rpc.store.ipblack.remark.null")]
        public string Remark { get; set; }

    }
}
