using WeDonekRpc.Client;
using RpcStore.RemoteModel.Control.Model;

namespace RpcStore.RemoteModel.Control
{
    /// <summary>
    /// 修改服务中心资料
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetControl : RpcRemote
    {
        /// <summary>
        /// 服务中心ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 服务中心资料
        /// </summary>
        public RpcControlDatum Datum { get; set; }
    }
}
