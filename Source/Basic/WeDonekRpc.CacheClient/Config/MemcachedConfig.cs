namespace WeDonekRpc.CacheClient.Config
{
    /// <summary>
    /// Memcached配置
    /// </summary>
    public class MemcachedConfig
    {
        /// <summary>
        /// 最小链接数
        /// </summary>
        public int MinPoolSize
        {
            get;
            set;
        } = 2;
        /// <summary>
        /// 最大链接数
        /// </summary>
        public int MaxPoolSize
        {
            get;
            set;
        } = 100;
        /// <summary>
        /// 链接地址
        /// </summary>
        public string[] ServerIp
        {
            get;
            set;
        }
        /// <summary>
        /// 登陆名
        /// </summary>
        public string UserName
        {
            get;
            set;
        }
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd
        {
            get;
            set;
        }
        /// <summary>
        /// 链接超时(秒)
        /// </summary>
        public int ConnectionTimeout { get; set; } = 3;
    }
}
