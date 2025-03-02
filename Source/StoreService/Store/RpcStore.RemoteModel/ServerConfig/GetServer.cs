using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerConfig
{
    /// <summary>
    /// 获取服务节点资料
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetServer : RpcRemote<Model.RemoteServerDatum>
    {
        /// <summary>
        /// 服务节点ID
        /// </summary>
        public long ServerId { get; set; }
    }
}
