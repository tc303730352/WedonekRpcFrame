using WeDonekRpc.Client;
using RpcStore.RemoteModel.Mer.Model;

namespace RpcStore.RemoteModel.Mer
{
    /// <summary>
    /// 查询服务集群
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QueryMer : BasicPage<RpcMer>
    {
        /// <summary>
        /// 集群名
        /// </summary>
        public string Name
        {
            get;
            set;
        }
    }
}
