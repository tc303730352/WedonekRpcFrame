using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.Trace.Model;
namespace Store.Gatewary.Modular.Model.Trace
{
    /// <summary>
    /// 查询链路信息 UI参数实体
    /// </summary>
    internal class UI_QueryTrace : BasicPage
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        [NullValidate("rpc.store.trace.query.null")]
        public TraceQuery Query { get; set; }

    }
}
