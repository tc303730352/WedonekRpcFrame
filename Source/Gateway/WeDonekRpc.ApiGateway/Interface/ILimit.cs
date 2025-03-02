using WeDonekRpc.ApiGateway.Config;

namespace WeDonekRpc.ApiGateway.Interface
{
    internal interface ILimit
    {
        GatewayLimitType LimitType { get; }
        bool IsLimit();
        void Refresh(int time);
    }
}
