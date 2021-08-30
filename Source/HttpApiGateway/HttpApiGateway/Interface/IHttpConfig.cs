using System;

using HttpApiGateway.Model;

namespace HttpApiGateway.Interface
{
        public interface IHttpConfig
        {
                /// <summary>
                /// 响应模板
                /// </summary>
                IApiResponseTemplate ApiTemplate { get; set; }
                /// <summary>
                /// 监听地址
                /// </summary>
                Uri Url { get; }

                /// <summary>
                /// 接口路由格式
                /// </summary>
                string ApiRouteFormat { get; }

                /// <summary>
                /// 最大请求长度
                /// </summary>
                public long MaxRequstLength { get; }

                /// <summary>
                /// 上传配置
                /// </summary>
                public UpConfig UpConfig { get; }
                /// <summary>
                /// 跨域配置
                /// </summary>
                public CrossDomainConfig CrossDomain { get; }
        }
}