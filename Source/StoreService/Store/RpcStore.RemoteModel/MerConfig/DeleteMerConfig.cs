using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.MerConfig
{
    /// <summary>
    /// 删除集群配置
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class DeleteMerConfig : RpcRemote
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id { get; set; }
    }
}
