namespace WeDonekRpc.ApiGateway.Config
{
    /// <summary>
    /// 时间窗限制
    /// </summary>
    public class LimitTimeWin
    {

        /// <summary>
        /// 间隔时间窗
        /// </summary>
        public short Interval { get; set; } = 1;

        /// <summary>
        /// 限制数量
        /// </summary>
        public int LimitNum { get; set; } = 100;
    }
}
