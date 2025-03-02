namespace WeDonekRpc.CacheClient.Config
{

    /// <summary>
    /// Redis配置
    /// </summary>
    public class RedisConfig
    {
        /// <summary>
        /// 链接配置
        /// </summary>
        public RedisConConfig ConConfig { get; set; }

        /// <summary>
        /// 哨兵节点
        /// </summary>
        public string[] Sentinels
        {
            get;
            set;
        }

        /// <summary>
        /// 是否只读
        /// false: 只获取master节点进行读写操作
        // true: 只获取可用slave节点进行只读操作
        /// </summary>
        public bool ReadOnly
        {
            get;
            set;
        }
        /// <summary>
        /// 链接列表
        /// </summary>
        public RedisConConfig[] ConList { get; set; }

    }
}
