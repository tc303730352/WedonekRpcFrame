using System;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.ApiGateway.Model;

using WeDonekRpc.WebSocketGateway.Interface;

namespace WeDonekRpc.WebSocketGateway.Model
{
    internal class RouteConfig : ApiConfig, IRouteConfig
    {
        public ApiRouteName Route { get; set; }
        public Action<IApiSocketService> AccreditVer { get; set; }
        public Type ApiEventType { get; set; }
    }
}
