using WeDonekRpc.Client;
using RpcStore.RemoteModel.RunState.Model;

namespace RpcStore.RemoteModel.RunState
{
    /// <summary>
    /// 获取服务节点运行状态
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetRunState : RpcRemote<ServerRunState>
    {
        /// <summary>
        /// 服务节点ID
        /// </summary>
        public long ServerId { get; set; }
    }
}
