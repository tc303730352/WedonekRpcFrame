using RpcHelper.Validate;

namespace Wedonek.RpcStore.Service.Model
{
        /// <summary>
        /// 降级配置
        /// </summary>
        public class ReduceInRankDatum
        {
                /// <summary>
                /// 触发限制错误数
                /// </summary>
                [NumValidate("rpc.reduce.limit.num.error", 1)]
                public int LimitNum
                {
                        get;
                        set;
                }
                /// <summary>
                /// 熔断错误数
                /// </summary>
                [NumValidate("rpc.reduce.fusing.error.num", 1)]
                public int FusingErrorNum
                {
                        get;
                        set;
                }

                /// <summary>
                /// 刷新统计数的时间(秒)
                /// </summary>
                [NumValidate("rpc.reduce.refresh.time.error", 1)]
                public int RefreshTime { get; set; }
                /// <summary>
                /// 最短融断时长
                /// </summary>
                [NumValidate("rpc.reduce.begin.time.error", 1)]
                public int BeginDuration
                {
                        get;
                        set;
                }
                /// <summary>
                /// 最长熔断时长
                /// </summary>
                [NumValidate("rpc.reduce.end.time.error", 1)]
                public int EndDuration
                {
                        get;
                        set;
                }
        }
}
