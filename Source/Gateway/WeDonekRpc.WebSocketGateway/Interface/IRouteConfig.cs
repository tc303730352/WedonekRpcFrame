using System;
using WeDonekRpc.ApiGateway.Interface;

namespace WeDonekRpc.WebSocketGateway.Interface
{
    public interface IRouteConfig : IApiConfig
    {
        /// <summary>
        /// 授权验证
        /// </summary>
        Action<IApiSocketService> AccreditVer { get; set; }

        Type ApiEventType { get; set; }
    }
}
