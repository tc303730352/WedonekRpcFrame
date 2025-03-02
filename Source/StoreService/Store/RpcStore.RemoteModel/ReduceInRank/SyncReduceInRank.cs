using WeDonekRpc.Client;
using RpcStore.RemoteModel.ReduceInRank.Model;

namespace RpcStore.RemoteModel.ReduceInRank
{
    /// <summary>
    /// 服务节点降级配置
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SyncReduceInRank : RpcRemote
    {
        /// <summary>
        /// 降级配置
        /// </summary>
        public ReduceInRankAdd Datum
        {
            get;
            set;
        }
    }
}
