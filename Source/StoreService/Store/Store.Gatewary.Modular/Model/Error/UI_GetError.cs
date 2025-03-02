using WeDonekRpc.Helper.Validate;
namespace Store.Gatewary.Modular.Model.Error
{
    /// <summary>
    /// 获取错误信息 UI参数实体
    /// </summary>
    internal class UI_GetError
    {
        /// <summary>
        /// 错误ID
        /// </summary>
        [NumValidate("rpc.store.error.errorId.error", 1)]
        public long ErrorId { get; set; }

        /// <summary>
        /// 语言类型
        /// </summary>
        [NullValidate("rpc.store.error.lang.null")]
        public string Lang { get; set; }

    }
}
