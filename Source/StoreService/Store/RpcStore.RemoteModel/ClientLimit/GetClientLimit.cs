using WeDonekRpc.Client;
using RpcStore.RemoteModel.ClientLimit.Model;

namespace RpcStore.RemoteModel.ClientLimit
{
    /// <summary>
    /// 获取服务节点限流配置
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetClientLimit : RpcRemote<ClientLimitData>
    {
        /// <summary>
        /// 集群ID
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点ID
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
    }
}
