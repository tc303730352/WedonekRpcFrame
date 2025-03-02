using WeDonekRpc.Helper.Validate;
using RpcStore.RemoteModel.SysConfig.Model;
namespace Store.Gatewary.Modular.Model.SysConfig
{
    /// <summary>
    /// 修改配置 UI参数实体
    /// </summary>
    internal class UI_SetSysConfig
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        [NumValidate("rpc.store.sysconfig.id.error", 1)]
        public long Id { get; set; }

        /// <summary>
        /// 配置值
        /// </summary>
        [NullValidate("rpc.store.sysconfig.configSet.null")]
        public SysConfigSet ConfigSet { get; set; }

    }
}
