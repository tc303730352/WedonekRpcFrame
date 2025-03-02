using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.ResourceModular.Model;
namespace Store.Gatewary.Modular.Model.ResourceModular
{
    /// <summary>
    /// 查询资源模块 UI参数实体
    /// </summary>
    internal class UI_QueryModular : BasicPage
    {
        /// <summary>
        /// 模块查询参数
        /// </summary>
        [NullValidate("rpc.store.resourcemodular.query.null")]
        public ModularQuery Query { get; set; }

    }
}
