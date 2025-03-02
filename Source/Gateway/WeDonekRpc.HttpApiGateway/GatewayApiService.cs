using System;
using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway
{
    internal class GatewayApiService : IGatewayApiService
    {
        public static GatewayApiService Service;

        public event Action<IApiHandler, IRoute> InitEvent;
        public event Action<IApiService, IRoute> RouteBeginEvent;
        public event Action<IApiService, IRoute> RouteEndEvent;
        public event Action<IApiHandler, IRoute, IApiService> EndEvent;
        public event Action<IApiService, IRoute> EndInitEvent;
        public event Action<IApiService, IRoute> BeginInitEvent;

        static GatewayApiService ()
        {
            Service = new GatewayApiService();
        }

        internal static void BeginInit ( IApiService service, IRoute route )
        {
            GatewayApiService.Service.BeginInitEvent?.Invoke(service, route);
        }
        internal static void EndRequest ( IApiHandler handler, IRoute route, IApiService service )
        {
            GatewayApiService.Service.EndEvent?.Invoke(handler, route, service);
        }
        internal static void RouteBegin ( IApiService service, IRoute route )
        {
            GatewayApiService.Service.RouteBeginEvent?.Invoke(service, route);
        }
        internal static void RouteEnd ( IApiService service, IRoute route )
        {
            GatewayApiService.Service.RouteEndEvent?.Invoke(service, route);
        }

        internal static void EndInit ( IApiService service, IRoute route )
        {
            GatewayApiService.Service.EndInitEvent?.Invoke(service, route);
        }

        internal static void Init ( IApiHandler handler, IRoute route )
        {
            GatewayApiService.Service.InitEvent?.Invoke(handler, route);
        }
    }
}
