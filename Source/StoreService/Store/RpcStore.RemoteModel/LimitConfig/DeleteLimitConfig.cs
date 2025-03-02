using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.LimitConfig
{
    /// <summary>
    /// 删除服务节点全局限流配置
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class DeleteLimitConfig : RpcRemote
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
