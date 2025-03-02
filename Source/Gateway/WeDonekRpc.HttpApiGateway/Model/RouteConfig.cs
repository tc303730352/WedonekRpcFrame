using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.HttpApiGateway.Interface;
using System;

namespace WeDonekRpc.HttpApiGateway.Model
{
    internal class RouteConfig : ApiConfig, IRouteConfig
    {
        public Type UpConfig { get;  set; }

        public Type ApiEventType { get;  set; }

        public ApiUpSet UpSet { get; set; } 
    }
}
