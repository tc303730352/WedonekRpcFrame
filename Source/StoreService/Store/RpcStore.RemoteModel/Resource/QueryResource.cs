using WeDonekRpc.Client;
using RpcStore.RemoteModel.Resource.Model;

namespace RpcStore.RemoteModel.Resource
{
    /// <summary>
    /// 查询资源信息
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QueryResource : BasicPage<ResourceDatum>
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        public ResourceQuery Query
        {
            get;
            set;
        }
    }
}
