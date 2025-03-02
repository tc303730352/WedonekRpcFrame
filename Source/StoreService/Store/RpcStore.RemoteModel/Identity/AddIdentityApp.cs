using WeDonekRpc.Client;
using RpcStore.RemoteModel.Identity.Model;

namespace RpcStore.RemoteModel.Identity
{
    /// <summary>
    /// 添加身份标识
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class AddIdentityApp : RpcRemote<long>
    {
        /// <summary>
        /// 身份标识资料
        /// </summary>
        public IdentityDatum Datum
        {
            get;
            set;
        }
    }
}
