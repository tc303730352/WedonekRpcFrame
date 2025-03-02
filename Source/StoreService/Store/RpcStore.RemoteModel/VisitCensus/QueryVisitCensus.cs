using WeDonekRpc.Client;
using RpcStore.RemoteModel.VisitCensus.Model;

namespace RpcStore.RemoteModel.VisitCensus
{
    /// <summary>
    /// 查询服务节点访问统计
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QueryVisitCensus : BasicPage<ServerVisitCensus>
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        public VisitCensusQuery Query
        {
            get;
            set;
        }
    }
}
