using WeDonekRpc.Helper.Validate;
using RpcStore.RemoteModel.IpBlack.Model;
namespace Store.Gatewary.Modular.Model.IpBlack
{
    /// <summary>
    /// 修改Ip黑名单 UI参数实体
    /// </summary>
    internal class UI_SetIpBlack
    {
        /// <summary>
        /// Ip黑名单ID
        /// </summary>
        [NumValidate("rpc.store.ipblack.id.error", 1)]
        public long Id { get; set; }

        /// <summary>
        /// 黑名单资料
        /// </summary>
        [NullValidate("rpc.store.ipblack.datum.null")]
        public IpBlackSet Datum { get; set; }

    }
}
