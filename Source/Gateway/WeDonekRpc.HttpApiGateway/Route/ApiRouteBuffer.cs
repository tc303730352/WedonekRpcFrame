using System;
using System.Collections.Generic;
using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway.Route
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

        public List<ApiBody> Apis { get; set; } = new List<ApiBody>();
    }
}
