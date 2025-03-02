using WeDonekRpc.ApiGateway.Config;

namespace WeDonekRpc.ApiGateway.Interface
{
    public interface ILimitConfig
    {
        LimitTimeWin FixedTime { get; set; }
        LimitTimeWin FlowTime { get; set; }
        GatewayLimitType LimitType { get; set; }
        TokenConfig Token { get; set; }
    }
}