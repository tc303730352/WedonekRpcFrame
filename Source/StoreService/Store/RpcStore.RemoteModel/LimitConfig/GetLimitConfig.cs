using WeDonekRpc.Client;
using WeDonekRpc.Model.Model;

namespace RpcStore.RemoteModel.LimitConfig
{
    /// <summary>
    /// 获取服务节点全局限流配置
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetLimitConfig : RpcRemote<ServerLimitConfig>
    {
        /// <summary>
        /// 服务节点ID
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
    }
}
