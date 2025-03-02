using WeDonekRpc.Client;
using RpcStore.RemoteModel.ResourceShield.Model;

namespace RpcStore.RemoteModel.ResourceShield
{
    /// <summary>
    /// 查询屏蔽的资源信息
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QueryResourceShield : BasicPage<ResourceShieldDatum>
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        public ResourceShieldQuery Query
        {
            get;
            set;
        }
    }
}
