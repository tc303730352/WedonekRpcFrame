using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.VisitCensus.Model;
namespace Store.Gatewary.Modular.Model.VisitCensus
{
    /// <summary>
    /// 查询服务节点访问统计 UI参数实体
    /// </summary>
    internal class UI_QueryVisitCensus : BasicPage
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        [NullValidate("rpc.store.visitcensus.query.null")]
        public VisitCensusQuery Query { get; set; }

    }
}
