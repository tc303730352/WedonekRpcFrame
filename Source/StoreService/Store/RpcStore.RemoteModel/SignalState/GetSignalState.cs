using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.SignalState
{
    /// <summary>
    /// 获取服务节点和各节点之间的通信状态
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetSignalState : RpcRemoteArray<Model.ServerSignalState>
    {
        public long ServerId { get; set; }
    }
}
