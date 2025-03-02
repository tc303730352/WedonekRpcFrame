using WeDonekRpc.Client;
using RpcStore.RemoteModel.Control.Model;

namespace RpcStore.RemoteModel.Control
{
    /// <summary>
    /// 获取服务中心信息
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetControl : RpcRemote<RpcControl>
    {
        /// <summary>
        /// 服务中心ID
        /// </summary>
        public int Id { get; set; }
    }
}
