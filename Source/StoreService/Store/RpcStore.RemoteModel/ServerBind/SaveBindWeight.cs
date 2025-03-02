using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerBind
{
    /// <summary>
    /// 设置绑定的服务节点负载均衡时的权重
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SaveBindWeight : RpcRemote
    {
        /// <summary>
        /// 负载均衡权重
        /// </summary>
        public Model.SaveWeight Weight
        {
            get;
            set;
        }
    }
}
