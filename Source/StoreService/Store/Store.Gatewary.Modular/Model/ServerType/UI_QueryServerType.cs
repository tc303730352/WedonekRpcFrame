using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.ServerType.Model;
namespace Store.Gatewary.Modular.Model.ServerType
{
    /// <summary>
    /// 查询服务类别 UI参数实体
    /// </summary>
    internal class UI_QueryServerType : BasicPage
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        [NullValidate("rpc.store.servertype.query.null")]
        public ServerTypeQuery Query { get; set; }

    }
}
