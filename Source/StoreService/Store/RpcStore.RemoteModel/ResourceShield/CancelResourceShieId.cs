using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ResourceShield
{
    /// <summary>
    /// 取消资源屏蔽
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class CancelResourceShieId : RpcRemote
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId
        {
            get;
            set;
        }
    }
}
