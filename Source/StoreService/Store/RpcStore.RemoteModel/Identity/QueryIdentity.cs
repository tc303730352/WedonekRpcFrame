using WeDonekRpc.Client;
using RpcStore.RemoteModel.Identity.Model;

namespace RpcStore.RemoteModel.Identity
{
    /// <summary>
    /// 查询身份标识
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QueryIdentity : BasicPage<IdentityApp>
    {
        /// <summary>
        /// 查询信息
        /// </summary>
        public IdentityQuery Query
        {
            get;
            set;
        }
    }
}
