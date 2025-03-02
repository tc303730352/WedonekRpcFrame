using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.Error.Model;
namespace Store.Gatewary.Modular.Model.Error
{
    /// <summary>
    /// 查询已有错误信息 UI参数实体
    /// </summary>
    internal class UI_QueryError : BasicPage
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        [NullValidate("rpc.store.error.query.null")]
        public ErrorQuery Query { get; set; }

    }
}
