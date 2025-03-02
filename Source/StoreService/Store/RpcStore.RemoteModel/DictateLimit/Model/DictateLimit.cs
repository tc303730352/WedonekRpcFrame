using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.DictateLimit.Model
{
    /// <summary>
    /// 指令限流配置
    /// </summary>
    public class DictateLimit
    {
        /// <summary>
        /// 指令Id
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 服务Id
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 限流指令
        /// </summary>
        public string Dictate
        {
            get;
            set;
        }
        /// <summary>
        /// 限流类型
        /// </summary>
        public ServerLimitType LimitType
        {
            get;
            set;
        }
        /// <summary>
        /// 最大流量
        /// </summary>
        public int LimitNum
        {
            get;
            set;
        }
        /// <summary>
        /// 窗口大小（秒）
        /// </summary>
        public short LimitTime
        {
            get;
            set;
        }


        /// <summary>
        /// 桶大小
        /// </summary>
        public short BucketSize
        {
            get;
            set;
        }
        /// <summary>
        /// 桶溢出速度
        /// </summary>
        public short BucketOutNum
        {
            get;
            set;
        }
        /// <summary>
        /// 令牌最大数
        /// </summary>
        public short TokenNum
        {
            get;
            set;
        }
        /// <summary>
        /// 每秒添加令牌数
        /// </summary>
        public short TokenInNum
        {
            get;
            set;
        }
    }
}
