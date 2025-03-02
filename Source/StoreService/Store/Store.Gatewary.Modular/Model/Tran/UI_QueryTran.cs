using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.Tran.Model;
namespace Store.Gatewary.Modular.Model.Tran
{
    /// <summary>
    /// 查询事务日志 UI参数实体
    /// </summary>
    internal class UI_QueryTran : BasicPage
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        [NullValidate("rpc.store.tran.query.null")]
        public TransactionQuery Query { get; set; }

    }
}
