using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.SysConfig.Model;
namespace Store.Gatewary.Modular.Model.SysConfig
{
    /// <summary>
    /// 查询系统配置 UI参数实体
    /// </summary>
    internal class UI_QuerySysConfig : BasicPage
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        [NullValidate("rpc.store.sysconfig.query.null")]
        public QuerySysParam Query { get; set; }

    }
}
