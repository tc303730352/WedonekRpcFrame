using System;
using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway.Route
{
    internal class ApiBody
    {
        public Type ApiEventType { get; set; }
        public Type UpConfigType { get; set; }
        public string Name { get; set; }
        public IApiRoute Route { get; set; }
    }
}
