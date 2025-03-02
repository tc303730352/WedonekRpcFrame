using WeDonekRpc.Helper.Validate;
using RpcStore.RemoteModel.DictateLimit.Model;
namespace Store.Gatewary.Modular.Model.DictateLimit
{
    /// <summary>
    /// 设置指令限流配置 UI参数实体
    /// </summary>
    internal class UI_SetDictateLimit
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        [NumValidate("rpc.store.dictatelimit.id.error", 1)]
        public long Id { get; set; }

        /// <summary>
        /// 指令限流资料
        /// </summary>
        [NullValidate("rpc.store.dictatelimit.datum.null")]
        public DictateLimitSet Datum { get; set; }

    }
}
