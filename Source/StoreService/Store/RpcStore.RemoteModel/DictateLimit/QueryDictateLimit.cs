using WeDonekRpc.Client;
using RpcStore.RemoteModel.DictateLimit.Model;

namespace RpcStore.RemoteModel.DictateLimit
{
    /// <summary>
    /// 查询指令限流配置
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QueryDictateLimit : BasicPage<Model.DictateLimit>
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        public DictateLimitQuery Query
        {
            get;
            set;
        }
    }
}
