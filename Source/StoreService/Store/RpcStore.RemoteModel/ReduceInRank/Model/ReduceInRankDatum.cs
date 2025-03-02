using WeDonekRpc.Helper.Validate;

namespace RpcStore.RemoteModel.ReduceInRank.Model
{
    /// <summary>
    /// 降级配置
    /// </summary>
    public class ReduceInRankDatum
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 触发限制错误数
        /// </summary>
        [NumValidate("rpc.store.reduce.limit.num.error", 1)]
        public int LimitNum
        {
            get;
            set;
        }
        /// <summary>
        /// 熔断错误数
        /// </summary>
        [NumValidate("rpc.store.reduce.fusing.error.num", 1)]
        public int FusingErrorNum
        {
            get;
            set;
        }

        /// <summary>
        /// 刷新统计数的时间(秒)
        /// </summary>
        [NumValidate("rpc.store.reduce.refresh.time.error", 1)]
        public int RefreshTime { get; set; }
        /// <summary>
        /// 最短融断时长
        /// </summary>
        [NumValidate("rpc.store.reduce.begin.time.error", 1)]
        public int BeginDuration
        {
            get;
            set;
        }
        /// <summary>
        /// 最长熔断时长
        /// </summary>
        [NumValidate("rpc.store.reduce.end.time.error", 1)]
        public int EndDuration
        {
            get;
            set;
        }
    }
}
