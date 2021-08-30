using System;

using ApiGateway.Model;

using WebSocketGateway.Interface;

namespace WebSocketGateway.Model
{
        internal class RouteConfig : ApiConfig, IRouteConfig
        {
                public Action<IApiSocketService> AccreditVer { get; set; }
        }
}
