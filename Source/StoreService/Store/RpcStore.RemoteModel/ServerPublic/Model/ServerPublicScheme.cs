namespace RpcStore.RemoteModel.ServerPublic.Model
{
    public class ServerPublicScheme
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// 方案名
        /// </summary>
        public string SchemeName { get; set; }
        /// <summary>
        /// 方案说明
        /// </summary>
        public string SchemeShow { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public SchemeStatus Status { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastTime { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
    }
}
