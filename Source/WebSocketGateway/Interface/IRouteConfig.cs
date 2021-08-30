using System;

using ApiGateway.Interface;

namespace WebSocketGateway.Interface
{
        public interface IRouteConfig: IApiConfig
        {
                public Action<IApiSocketService> AccreditVer { get; set; }
        }
}
