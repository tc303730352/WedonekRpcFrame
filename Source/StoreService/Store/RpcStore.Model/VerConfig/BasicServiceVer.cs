namespace RpcStore.Model.VerConfig
{
    public class BasicServiceVer
    {
        /// <summary>
        /// 版本Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public long VerNum { get; set; }
        /// <summary>
        /// 系统节点类型
        /// </summary>
        public long SystemTypeId { get; set; }
        /// <summary>
        /// 版本名
        /// </summary>
        public string VerTitle { get; set; }
        /// <summary>
        /// 版本说明
        /// </summary>
        public string VerShow { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
    }
}
