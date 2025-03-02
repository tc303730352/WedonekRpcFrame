using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerConfig.Model;

namespace RpcStore.RemoteModel.ServerConfig
{
    /// <summary>
    /// 新增服务节点
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class AddServer : RpcRemote<long>
    {
        /// <summary>
        /// 节点资料
        /// </summary>
        public ServerConfigAdd Datum
        {
            get;
            set;
        }
    }
}
