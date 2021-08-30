using RpcModel;

namespace Wedonek.RpcStore.Service.Model
{
        /// <summary>
        /// 修改指令限流
        /// </summary>
        public class SetDictateLimit
        {
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
                /// 窗口大小
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
