using System;
using WeDonekRpc.Client.Attr;

namespace WeDonekRpc.HttpApiGateway.Interface
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    public interface IGatewayApiService
    {
        /// <summary>
        /// 请求初始化
        /// </summary>
        event Action<IApiHandler, IRoute> InitEvent;

        /// <summary>
        /// 路由开始执行事件
        /// </summary>
        event Action<IApiService, IRoute> RouteBeginEvent;
        /// <summary>
        /// 路由结束执行事件
        /// </summary>
        event Action<IApiService, IRoute> RouteEndEvent;

        /// <summary>
        /// 请求结束事件
        /// </summary>
        event Action<IApiHandler, IRoute, IApiService> EndEvent;

        /// <summary>
        /// 结束初始化事件
        /// </summary>
        event Action<IApiService, IRoute> EndInitEvent;

        /// <summary>
        /// 开始初始化事件
        /// </summary>
        event Action<IApiService, IRoute> BeginInitEvent;
    }
}