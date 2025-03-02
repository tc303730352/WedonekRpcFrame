using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.IpBlack.Model;
namespace Store.Gatewary.Modular.Model.IpBlack
{
    /// <summary>
    /// 查询Ip黑名单 UI参数实体
    /// </summary>
    internal class UI_QueryIpBlack : BasicPage
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        [NullValidate("rpc.store.ipblack.query.null")]
        public IpBlackQuery Query { get; set; }

    }
}
