using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Broadcast
{
    /// <summary>
    /// 更新节点状态
    /// </summary>
    [IRemoteConfig("Rpc_ResetState", false)]
    public class ResetServiceState : RpcRemote
    {
        public RpcServiceState ServiceState
        {
            get;
            set;
        }
    }
}
