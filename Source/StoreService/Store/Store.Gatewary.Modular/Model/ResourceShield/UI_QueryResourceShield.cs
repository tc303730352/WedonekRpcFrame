using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.ResourceShield.Model;
namespace Store.Gatewary.Modular.Model.ResourceShield
{
    /// <summary>
    /// 查询屏蔽的资源信息 UI参数实体
    /// </summary>
    internal class UI_QueryResourceShield : BasicPage
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        [NullValidate("rpc.store.resourceshield.query.null")]
        public ResourceShieldQuery Query { get; set; }

    }
}
