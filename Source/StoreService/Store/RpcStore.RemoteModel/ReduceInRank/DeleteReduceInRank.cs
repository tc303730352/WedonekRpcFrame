using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ReduceInRank
{
    /// <summary>
    /// 删除服务节点降级配置
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class DeleteReduceInRank : RpcRemote
    { 
        /// <summary>
       /// 数据ID
       /// </summary>
        public long Id { get; set; }
    }
}
