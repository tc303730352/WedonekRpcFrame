using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.Tran
{
    /// <summary>
    /// 获取主事务下的事务日志
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetTranTree : RpcRemoteArray<Model.TransactionTree>
    {
        /// <summary>
        /// 主事务ID
        /// </summary>
        public long TranId
        {
            get;
            set;
        }
    }
}
