namespace RpcStore.RemoteModel.Identity.Model
{
    public class IdentityApp
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 应用名
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// 应用说明
        /// </summary>
        public string AppShow { get; set; }
        /// <summary>
        /// 应用有效期
        /// </summary>
        public DateTime? EffectiveDate { get; set; }
        /// <summary>
        /// 应用是否启用
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
