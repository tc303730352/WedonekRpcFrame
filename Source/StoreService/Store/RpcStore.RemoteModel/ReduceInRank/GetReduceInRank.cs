using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ReduceInRank
{
    /// <summary>
    /// 获取集群下某个服务节点降级配置
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetReduceInRank : RpcRemote<Model.ReduceInRankConfig>
    {
        /// <summary>
        /// 集群ID
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
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
