using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.WebSocketGateway.Route;

namespace WeDonekRpc.WebSocketGateway.Interface
{
    public interface IWebSocketGatewayOption
    {
        IocBuffer IocBuffer { get; }
        IGatewayOption Option { get; }
        RouteBuffer Route { get; }

        void RegModular (IModular modular);
    }
}