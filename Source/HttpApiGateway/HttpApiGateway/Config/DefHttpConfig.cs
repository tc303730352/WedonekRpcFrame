using System;

using HttpApiGateway.Model;

namespace HttpApiGateway.Config
{
        internal class DefHttpConfig
        {
                /// <summary>
                /// 网关地址
                /// </summary>
                public Uri Url { get; set; }
                /// <summary>
                /// API路径格式
                /// </summary>
                public string ApiRouteFormat { get; set; } = "/api/{controller}/{name}";
                /// <summary>
                /// 最大请求大小
                /// </summary>
                public long MaxRequstLength { get; set; }

                public UpConfig UpConfig { get; set; } = new UpConfig();

                public CrossDomainConfig CrossDomain { get; set; } = new CrossDomainConfig();
        }
}
