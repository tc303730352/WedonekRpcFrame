using WeDonekRpc.Helper.Validate;
namespace Store.Gatewary.Modular.Model.SysConfig
{
    /// <summary>
    /// 设置配置的启用状态 UI参数实体
    /// </summary>
    internal class UI_SetSysConfigIsEnable
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        [NumValidate("rpc.store.sysconfig.id.error", 1)]
        public long Id { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

    }
}
