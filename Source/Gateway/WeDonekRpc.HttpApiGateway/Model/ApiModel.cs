using WeDonekRpc.ApiGateway;
using WeDonekRpc.ApiGateway.Model;
using WeDonekRpc.HttpApiGateway.Interface;
using System;
using System.Reflection;

namespace WeDonekRpc.HttpApiGateway.Model
{
    internal class ApiModel : IApiModel
    {
        public Type ApiEventType { get; set; }
        public string ApiUri { get; set; }
        public Type Type
        {
            get;
            set;
        }
        public string[] Prower { get; set; }

        public bool IsAccredit { get; set; }

        public bool IsEnable { get; set; } = true;

        public MethodInfo Method
        {
            get;
            set;
        }
        public Type UpConfig { get; set; }

        public ApiUpSet UpSet { get; set; }

        public ApiType ApiType { get; set; }

        public bool IsRegex { get; set; }
    }
}
