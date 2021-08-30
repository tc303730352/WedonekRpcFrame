using System;

using HttpApiGateway.Interface;
using HttpApiGateway.Model;

using RpcClient.Interface;

namespace HttpApiGateway.Config
{
        internal class HttpConfig : IHttpConfig
        {
                private static DefHttpConfig _DefConfig = null;
                static HttpConfig()
                {
                        RpcClient.RpcClient.Config.AddRefreshEvent(_Refresh);
                }

                private static void _Refresh(IConfigServer config, string name)
                {
                        if (name.StartsWith("gateway:http") || name == string.Empty)
                        {
                                _DefConfig = RpcClient.RpcClient.Config.GetConfigVal<DefHttpConfig>("gateway:http", new DefHttpConfig());
                        }
                }

                /// <summary>
                /// Api响应模板
                /// </summary>
                public IApiResponseTemplate ApiTemplate
                {
                        get;
                        set;
                } = new Response.ApiResponseTemplate();
                /// <summary>
                /// 网关地址
                /// </summary>
                public Uri Url => _DefConfig.Url;

                public string ApiRouteFormat => _DefConfig.ApiRouteFormat;
                /// <summary>
                /// 最大请求长度
                /// </summary>
                public long MaxRequstLength => _DefConfig.MaxRequstLength;

                public UpConfig UpConfig => _DefConfig.UpConfig;

                public CrossDomainConfig CrossDomain => _DefConfig.CrossDomain;
        }
}
