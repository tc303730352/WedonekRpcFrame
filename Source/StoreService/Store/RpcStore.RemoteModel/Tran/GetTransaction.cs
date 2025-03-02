using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.Tran
{
    /// <summary>
    /// 获取单个事务信息
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetTransaction : RpcRemote<Model.TransactionData>
    {
        /// <summary>
        /// 事务ID
        /// </summary>
        public long TranId { get; set; }
    }
}
