using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.ServerConfig
{
    /// <summary>
    /// 设置服务节点状态
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetServiceState : RpcRemote
    {
        /// <summary>
        /// 节点ID
        /// </summary>
        public long ServiceId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public RpcServiceState State { get; set; }
    }
}
