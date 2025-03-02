using WeDonekRpc.Client;
using RpcStore.RemoteModel.ResourceModular.Model;

namespace RpcStore.RemoteModel.ResourceModular
{
    /// <summary>
    /// 查询资源模块
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QueryModular : BasicPage<ResourceModularDatum>
    {
        /// <summary>
        /// 模块查询参数
        /// </summary>
        public Model.ModularQuery Query
        {
            get;
            set;
        }
    }
}
