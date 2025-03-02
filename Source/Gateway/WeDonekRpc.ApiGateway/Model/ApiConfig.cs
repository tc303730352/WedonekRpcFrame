
using System;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.ApiGateway.Interface;

namespace WeDonekRpc.ApiGateway.Model
{
    public class ApiConfig : IApiConfig
    {
        public bool? IsEnable { get; set; }
        public bool IsAccredit { get; set; }
        public string[] Prower { get; set; }
        public Type Type { get; set; }
        public ApiRouteName Route { get; set; }
    }
}
