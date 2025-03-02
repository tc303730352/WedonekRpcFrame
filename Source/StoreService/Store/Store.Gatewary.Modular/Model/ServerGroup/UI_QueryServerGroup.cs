using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
namespace Store.Gatewary.Modular.Model.ServerGroup
{
    /// <summary>
    /// 查询服务组 UI参数实体
    /// </summary>
    internal class UI_QueryServerGroup : BasicPage
    {
        /// <summary>
        /// 名字
        /// </summary>
        [NullValidate("rpc.store.servergroup.name.null")]
        public string Name { get; set; }

    }
}
