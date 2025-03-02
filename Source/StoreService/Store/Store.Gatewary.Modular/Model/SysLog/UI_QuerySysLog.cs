using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.SysLog.Model;
namespace Store.Gatewary.Modular.Model.SysLog
{
    /// <summary>
    /// 查询日志 UI参数实体
    /// </summary>
    internal class UI_QuerySysLog : BasicPage
    {
        /// <summary>
        /// 
        /// </summary>
        [NullValidate("rpc.store.syslog.query.null")]
        public SysLogQuery Query { get; set; }

    }
}
