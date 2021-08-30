
using System;

using ApiGateway.Interface;

namespace ApiGateway.Model
{
        public class ApiConfig : IApiConfig
        {
                public bool IsAccredit { get; set; }
                public string Name { get; set; }
                public string Prower { get; set; }
                public Type Type { get; set; }
        }
}
