using WeDonekRpc.Model;
using RpcStore.RemoteModel.VisitCensus.Model;
using System;

namespace Store.Gatewary.Modular.Interface
{
    public interface IVisitCensusService
    {
        /// <summary>
        /// 查询服务节点访问统计
        /// </summary>
        /// <param name="query">查询参数</param>
        /// <param name="paging">分页参数</param>
        /// <param name="count">数据总数</param>
        /// <returns>服务节点的访问统计</returns>
        ServerVisitCensus[] QueryVisitCensus(VisitCensusQuery query, IBasicPage paging, out int count);

        /// <summary>
        /// 重置服务节点的访问统计
        /// </summary>
        /// <param name="serverId">服务节点ID</param>
        void ResetVisitCensus(long serverId);

        /// <summary>
        /// 设置服务节点的访问统计备注信息
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <param name="show">备注说明</param>
        void SetVisitCensusShow(long id, string show);

    }
}
