using System;
using System.Collections.Generic;
using WeDonekRpc.WebSocketGateway.Interface;

namespace WeDonekRpc.WebSocketGateway.Route
{
    internal class ApiRouteBuffer
    {
        public Type Form
        {
            get;
            set;
        }
        public Type To
        {
            get;
            set;
        }
        public string Name { get; set; }

        public IRouteConfig Config { get; set; }

        public List<ApiHandler> Apis { get; set; } = [];
    }
}
