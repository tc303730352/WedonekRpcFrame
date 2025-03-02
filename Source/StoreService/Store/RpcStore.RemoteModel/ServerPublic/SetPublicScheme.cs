using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerPublic.Model;

namespace RpcStore.RemoteModel.ServerPublic
{
    /// <summary>
    /// 设置发布方案
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetPublicScheme : RpcRemote<bool>
    {
        /// <summary>
        /// 方案ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 方案
        /// </summary>
        public PublicScheme Scheme { get; set; }
    }
}
