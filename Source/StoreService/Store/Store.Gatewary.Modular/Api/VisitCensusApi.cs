using RpcStore.RemoteModel.VisitCensus.Model;
using Store.Gatewary.Modular.Interface;
using Store.Gatewary.Modular.Model.VisitCensus;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway;

namespace Store.Gatewary.Modular.Api
{
    /// <summary>
    /// 服务节点的访问统计
    /// </summary>
    internal class VisitCensusApi : ApiController
    {
        private readonly IVisitCensusService _Service;
        public VisitCensusApi (IVisitCensusService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 查询服务节点访问统计
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns>服务节点的访问统计</returns>
        public PagingResult<ServerVisitCensus> Query ([NullValidate("rpc.store.visitcensus.param.null")] UI_QueryVisitCensus param)
        {
            ServerVisitCensus[] results = this._Service.QueryVisitCensus(param.Query, param, out int count);
            return new PagingResult<ServerVisitCensus>(count, results);
        }

        /// <summary>
        /// 重置服务节点的访问统计
        /// </summary>
        /// <param name="serverId">服务节点ID</param>
        public void Reset ([NumValidate("rpc.store.visitcensus.serverId.error", 1)] long serverId)
        {
            this._Service.ResetVisitCensus(serverId);
        }

        /// <summary>
        /// 设置服务节点的访问统计备注信息
        /// </summary>
        /// <param name="param">参数</param>
        [ApiPrower("rpc.store.admin")]
        public void SetShow ([NullValidate("rpc.store.visitcensus.param.null")] UI_SetVisitCensusShow param)
        {
            this._Service.SetVisitCensusShow(param.Id, param.Show);
        }

    }
}
