using WeDonekRpc.ApiGateway.Config;
using WeDonekRpc.Client.Attr;

namespace WeDonekRpc.ApiGateway.Interface
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    public interface IGatewayIdentityConfig
    {
        string ParamName { get; }
        IdentityReadMode ReadMode { get; }
    }
}