using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerBind.Model;

namespace RpcStore.RemoteModel.ServerBind
{
    /// <summary>
    /// 修改集群绑定的服务节点列表
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetBindServer : RpcRemote
    {
        /// <summary>
        /// 绑定信息
        /// </summary>
        public BindServer Bind
        {
            get;
            set;
        }
    }
}
