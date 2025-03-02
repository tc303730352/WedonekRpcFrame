using WeDonekRpc.Helper.Validate;
namespace Store.Gatewary.Modular.Model.ServerGroup
{
    /// <summary>
    /// 修改服务组名字 UI参数实体
    /// </summary>
    internal class UI_SetServerGroup
    {
        /// <summary>
        /// 服务组ID
        /// </summary>
        [NumValidate("rpc.store.servergroup.id.error", 1)]
        public long Id { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        [NullValidate("rpc.store.servergroup.name.null")]
        public string Name { get; set; }

    }
}
