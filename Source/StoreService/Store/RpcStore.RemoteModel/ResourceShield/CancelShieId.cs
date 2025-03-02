using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ResourceShield
{
    /// <summary>
    /// 取消屏蔽
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class CancelShieId : RpcRemote
    {
        /// <summary>
        /// 取消屏蔽
        /// </summary>
        public long Id { get; set; }
    }
}
