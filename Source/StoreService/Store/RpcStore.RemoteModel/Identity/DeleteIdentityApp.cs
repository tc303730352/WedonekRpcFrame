using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.Identity
{
    /// <summary>
    /// 删除身份标识
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class DeleteIdentityApp : RpcRemote
    {
        /// <summary>
        /// 标识ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }
    }
}
