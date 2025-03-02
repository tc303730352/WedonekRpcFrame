using WeDonekRpc.Client;
using RpcStore.RemoteModel.DictateLimit.Model;

namespace RpcStore.RemoteModel.DictateLimit
{
    /// <summary>
    /// 设置指令限流配置
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetDictateLimit : RpcRemote
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 指令限流资料
        /// </summary>
        public DictateLimitSet Datum
        {
            get;
            set;
        }
    }
}
