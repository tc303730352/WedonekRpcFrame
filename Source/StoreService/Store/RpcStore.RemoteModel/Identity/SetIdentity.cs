using WeDonekRpc.Client;
using RpcStore.RemoteModel.Identity.Model;

namespace RpcStore.RemoteModel.Identity
{
    /// <summary>
    /// 修改身份标识
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetIdentity : RpcRemote
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }
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
