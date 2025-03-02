using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.ApiGateway.Model;
using System;

namespace WeDonekRpc.HttpApiGateway.Interface
{
    public interface IRouteConfig : IApiConfig
    {
        /// <summary>
        /// 上传设置
        /// </summary>
        Type UpConfig { get; }
        /// <summary>
        /// 上传配置
        /// </summary>
        ApiUpSet UpSet { get; }
        /// <summary>
        /// Api事件类型
        /// </summary>
        Type ApiEventType { get; }

    }
}