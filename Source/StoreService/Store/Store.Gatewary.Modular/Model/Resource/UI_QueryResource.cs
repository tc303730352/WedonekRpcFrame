using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.Resource.Model;
namespace Store.Gatewary.Modular.Model.Resource
{
    /// <summary>
    /// 查询资源信息 UI参数实体
    /// </summary>
    internal class UI_QueryResource : BasicPage
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        [NullValidate("rpc.store.resource.query.null")]
        public ResourceQuery Query { get; set; }

    }
}
