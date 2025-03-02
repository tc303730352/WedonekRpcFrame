using WeDonekRpc.Client;
using RpcStore.RemoteModel.Error.Model;

namespace RpcStore.RemoteModel.Error
{
    /// <summary>
    /// 查询已有错误信息
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QueryError : BasicPage<ErrorData>
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        public ErrorQuery Query
        {
            get;
            set;
        }
    }
}
