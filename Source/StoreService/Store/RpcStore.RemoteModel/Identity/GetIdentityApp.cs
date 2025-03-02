using WeDonekRpc.Client;
using RpcStore.RemoteModel.Identity.Model;

namespace RpcStore.RemoteModel.Identity
{
    /// <summary>
    /// 获取身份标识
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetIdentityApp : RpcRemote<IdentityAppData>
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id { get; set; }
    }
}
