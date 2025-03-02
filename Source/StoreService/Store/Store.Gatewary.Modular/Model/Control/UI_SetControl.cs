using WeDonekRpc.Helper.Validate;
using RpcStore.RemoteModel.Control.Model;
namespace Store.Gatewary.Modular.Model.Control
{
    /// <summary>
    /// 修改服务中心资料 UI参数实体
    /// </summary>
    internal class UI_SetControl
    {
        /// <summary>
        /// 服务中心ID
        /// </summary>
        [NumValidate("rpc.store.control.id.error", 1)]
        public int Id { get; set; }

        /// <summary>
        /// 服务中心资料
        /// </summary>
        [NullValidate("rpc.store.control.datum.null")]
        public RpcControlDatum Datum { get; set; }

    }
}
