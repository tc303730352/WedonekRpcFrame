using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerConfig.Model;

namespace RpcStore.RemoteModel.ServerConfig
{
    /// <summary>
    /// 修改服务节点资料
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetServer : RpcRemote
    {
        /// <summary>
        /// 服务节点
        /// </summary>
        public long ServerId { get; set; }
        /// <summary>
        /// 修改的资料
        /// </summary>
        public ServerConfigSet Datum { get; set; }
    }
}
