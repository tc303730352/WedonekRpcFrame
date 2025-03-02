using WeDonekRpc.Client;
using RpcStore.RemoteModel.DictateLimit.Model;

namespace RpcStore.RemoteModel.DictateLimit
{
    /// <summary>
    /// 添加服务节点指令限流配置
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class AddDictateLimit : RpcRemote<long>
    {
        /// <summary>
        /// 指令限流配置
        /// </summary>
        public DictateLimitAdd Datum
        {
            get;
            set;
        }
    }
}
