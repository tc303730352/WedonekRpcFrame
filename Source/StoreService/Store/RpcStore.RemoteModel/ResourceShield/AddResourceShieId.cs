using WeDonekRpc.Client;
using RpcStore.RemoteModel.ResourceShield.Model;

namespace RpcStore.RemoteModel.ResourceShield
{
    /// <summary>
    /// 同步屏蔽信息(添加或修改)
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class AddResourceShieId : RpcRemote
    {
        /// <summary>
        /// 需要屏蔽的资料
        /// </summary>
        public ResourceShieldAdd Datum
        {
            get;
            set;
        }
    }
}
