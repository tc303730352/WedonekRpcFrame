using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.MerConfig
{
    /// <summary>
    /// 获取单个集群配置
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetMerConfig : RpcRemote<Model.RpcMerConfig>
    {
        /// <summary>
        /// 集群ID
        /// </summary>
        public long RpcMerId { get; set; }
        /// <summary>
        /// 服务类型ID
        /// </summary>
        public long SystemTypeId { get; set; }
    }
}
