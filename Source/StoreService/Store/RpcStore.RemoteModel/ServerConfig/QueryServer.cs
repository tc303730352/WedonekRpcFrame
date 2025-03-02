using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerConfig.Model;

namespace RpcStore.RemoteModel.ServerConfig
{
    /// <summary>
    /// 查询服务节点
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QueryServer : BasicPage<RemoteServer>
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        public ServerConfigQuery Query
        {
            get;
            set;
        }
    }
}
