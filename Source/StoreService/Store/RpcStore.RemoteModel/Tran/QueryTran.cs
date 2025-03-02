using WeDonekRpc.Client;
using RpcStore.RemoteModel.Tran.Model;

namespace RpcStore.RemoteModel.Tran
{
    /// <summary>
    /// 查询事务日志
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QueryTran : BasicPage<TransactionLog>
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        public TransactionQuery Query
        {
            get;
            set;
        }
    }
}
