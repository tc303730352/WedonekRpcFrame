using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.Identity.Model;
namespace Store.Gatewary.Modular.Model.Identity
{
    /// <summary>
    /// 查询身份标识 UI参数实体
    /// </summary>
    internal class UI_QueryIdentity : BasicPage
    {
        /// <summary>
        /// 查询信息
        /// </summary>
        [NullValidate("rpc.store.identity.query.null")]
        public IdentityQuery Query { get; set; }

    }
}
