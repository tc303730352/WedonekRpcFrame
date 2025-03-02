using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.DictateLimit.Model;
namespace Store.Gatewary.Modular.Model.DictateLimit
{
    /// <summary>
    /// 查询指令限流配置 UI参数实体
    /// </summary>
    internal class UI_QueryDictateLimit : BasicPage
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        [NullValidate("rpc.store.dictatelimit.query.null")]
        public DictateLimitQuery Query { get; set; }

    }
}
