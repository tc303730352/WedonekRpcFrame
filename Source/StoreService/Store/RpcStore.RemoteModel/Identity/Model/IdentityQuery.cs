namespace RpcStore.RemoteModel.Identity.Model
{
    /// <summary>
    /// 身份标识查询参数
    /// </summary>
    public class IdentityQuery
    {
        /// <summary>
        /// 应用名
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 有效期开始时间
        /// </summary>
        public DateTime? Begin { get; set; }
        /// <summary>
        /// 有效期截止时间
        /// </summary>
        public DateTime? End { get; set; }
    }
}
