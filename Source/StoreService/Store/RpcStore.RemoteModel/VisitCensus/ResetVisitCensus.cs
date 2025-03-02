using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.VisitCensus
{
    /// <summary>
    /// 重置服务节点的访问统计
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class ResetVisitCensus : RpcRemote
    {
        /// <summary>
        /// 服务节点ID
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
    }
}
