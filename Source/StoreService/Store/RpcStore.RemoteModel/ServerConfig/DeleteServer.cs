using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerConfig
{
    /// <summary>
    /// 删除节点
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class DeleteServer : RpcRemote
    {
        /// <summary>
        /// 服务节点ID
        /// </summary>
        public long ServerId { get; set; }
    }
}
