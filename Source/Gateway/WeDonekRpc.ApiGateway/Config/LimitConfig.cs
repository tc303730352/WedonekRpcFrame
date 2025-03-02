using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client.Attr;

namespace WeDonekRpc.ApiGateway.Config
{
    public enum GatewayLimitType
    {
        不启用 = 0,
        固定时间窗 = 1,
        流动时间窗 = 2,
        令牌桶 = 3
    }
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class LimitConfig : ILimitConfig
    {

        /// <summary>
        /// 限定方式
        /// </summary>
        public GatewayLimitType LimitType { get; set; } = GatewayLimitType.不启用;

        /// <summary>
        /// 固定时间窗
        /// </summary>
        public LimitTimeWin FixedTime { get; set; }

        /// <summary>
        /// 浮动时间窗
        /// </summary>
        public LimitTimeWin FlowTime { get; set; }
        /// <summary>
        /// 令牌桶配置
        /// </summary>
        public TokenConfig Token { get; set; }
    }
}
