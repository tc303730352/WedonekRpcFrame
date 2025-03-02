namespace WeDonekRpc.Model.Server
{
    /// <summary>
    /// 降级配置
    /// </summary>
    public class ReduceInRank
    {
        /// <summary>
        /// 是否启动降级
        /// </summary>
        public bool IsEnable
        {
            get;
            set;
        } = false;
        /// <summary>
        /// 触发限制错误数
        /// </summary>
        public int LimitNum
        {
            get;
            set;
        } = 10;

        /// <summary>
        /// 链接失败触发熔断次数
        /// </summary>
        public int FusingErrorNum
        {
            get;
            set;
        } = 2;

        /// <summary>
        /// 刷新统计数的时间(秒)
        /// </summary>
        public int RefreshTime { get; set; } = 2;
        /// <summary>
        /// 最短融断时长
        /// </summary>
        public int BeginDuration
        {
            get;
            set;
        } = 1;
        /// <summary>
        /// 最长熔断时长
        /// </summary>
        public int EndDuration
        {
            get;
            set;
        } = 5;
    }
}
