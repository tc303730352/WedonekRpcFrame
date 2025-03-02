using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.BroadcastErrorLog.Model;
namespace Store.Gatewary.Modular.Model.BroadcastErrorLog
{
    /// <summary>
    /// 查询广播错误日志 UI参数实体
    /// </summary>
    internal class UI_QueryBroadcastLog : BasicPage
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        [NullValidate("rpc.store.broadcasterrorlog.query.null")]
        public BroadcastErrorQuery Query { get; set; }

        /// <summary>
        /// 返回错误语言类型
        /// </summary>
        [NullValidate("rpc.store.broadcasterrorlog.lang.null")]
        public string Lang { get; set; }

    }
}
