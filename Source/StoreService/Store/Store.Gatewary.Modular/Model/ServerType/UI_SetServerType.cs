using WeDonekRpc.Helper.Validate;
using RpcStore.RemoteModel.ServerType.Model;
namespace Store.Gatewary.Modular.Model.ServerType
{
    /// <summary>
    /// 修改节点类型资料 UI参数实体
    /// </summary>
    internal class UI_SetServerType
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        [NumValidate("rpc.store.servertype.id.error", 1)]
        public long Id { get; set; }

        /// <summary>
        /// 类别资料
        /// </summary>
        [NullValidate("rpc.store.servertype.datum.null")]
        public ServerTypeSet Datum { get; set; }

    }
}
