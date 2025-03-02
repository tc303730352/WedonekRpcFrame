using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerBind.Model;

namespace RpcStore.RemoteModel.ServerBind
{
    /// <summary>
    /// 查询集群绑定的服务节点
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QueryBindServer : BasicPage<BindRemoteServer>
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
        /// 查询参数
        /// </summary>
        public BindQueryParam Query
        {
            get;
            set;
        }
    }
}
