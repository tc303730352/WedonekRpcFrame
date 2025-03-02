namespace WeDonekRpc.CacheClient.Config
{
    /// <summary>
    /// 缓存配置
    /// </summary>
    public class CacheConfig
    {
        /// <summary>
        /// 缓存Key
        /// </summary>
        public string SysKey
        {
            get;
            set;
        }
        /// <summary>
        /// Redis配置
        /// </summary>
        public RedisConfig Redis
        {
            get;
            set;
        } = new RedisConfig();
        /// <summary>
        /// Memcached配置
        /// </summary>
        public MemcachedConfig Memcached
        {
            get;
            set;
        } = new MemcachedConfig();
    }
}
