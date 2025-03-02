using WeDonekRpc.ApiGateway.Config;
using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.ApiGateway.Limit;
using WeDonekRpc.Helper;

namespace WeDonekRpc.ApiGateway.Helper
{
    internal static class LimitHelper
    {
        public static bool CheckIsLimit (this INodeLimitConfig config, string path)
        {
            if (config.LimitType == GatewayLimitType.不启用)
            {
                return false;
            }
            else if (config.LimitRange == NodeLimitRange.全局)
            {
                return !config.Excludes.IsExists(path);
            }
            return config.Limits.IsExists(path);
        }
        public static ILimit GetLimit (ILimitConfig config)
        {
            if (config.LimitType == GatewayLimitType.固定时间窗)
            {
                return new FixedTimeLimit(config.FixedTime);
            }
            else if (config.LimitType == GatewayLimitType.流动时间窗)
            {
                return new SlideTimeLimit(config.FlowTime);
            }
            else
            {
                return new TokenLimit(config.Token);
            }
        }
    }
}
