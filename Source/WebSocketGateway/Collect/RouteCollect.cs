using System;
using System.Collections.Concurrent;
using System.Reflection;

using ApiGateway;
using ApiGateway.Attr;

using HttpWebSocket.Interface;

using RpcHelper;

using WebSocketGateway.Helper;
using WebSocketGateway.Interface;
using WebSocketGateway.Model;

namespace WebSocketGateway.Collect
{
        internal class RouteCollect
        {
                private static readonly ConcurrentDictionary<string, ApiHandler> _RouteApi = new ConcurrentDictionary<string, ApiHandler>();
                #region 注册路由
                private static IRouteConfig _RegApi(Type type, IApiModular modular, Action<IRoute> action)
                {
                        ApiPrower prower = (ApiPrower)type.GetCustomAttribute(ApiPublicDict.ProwerAttr);
                        ApiRouteName route = (ApiRouteName)type.GetCustomAttribute(ApiPublicDict.ApiRouteName);
                        RouteConfig config = new RouteConfig
                        {
                                Name = route == null ? type.Name : route.Value,
                                Type = type,
                        };
                        if (prower != null)
                        {
                                config.IsAccredit = prower.IsAccredit;
                                config.Prower = prower.Prower;
                        }
                        GatewayService.RegModular(modular, type);
                        MethodInfo[] methods = type.GetMethods();
                        methods.ForEach(a =>
                        {
                                if (a.IsPublic && a.DeclaringType == type)
                                {
                                        _RegApi(a, config, modular, action);
                                }
                        });
                        return config;
                }
                private static void _RegApi(MethodInfo method, IRouteConfig config, IApiModular modular, Action<IRoute> action)
                {
                        ApiModel model = RouteHelper.GetApiParam(method, config, modular);
                        if (RouteHelper.GetApi(model, out ISocketApi api))
                        {
                                ApiHandler handler = new ApiHandler(api, model, config, modular);
                                if (_RouteApi.TryAdd(api.LocalPath, handler))
                                {
                                        handler.RegApi();
                                        action(handler.Route);
                                }
                        }
                }

                public static void RegApi(Type type, IApiController api, IApiModular modular, Action<IRoute> action)
                {
                        IRouteConfig config = _RegApi(type, modular, action);
                        api?.Install(config);
                }
                public static void RegApi(Type type, IApiModular modular, Action<IRoute> action)
                {
                        _RegApi(type, modular, action);
                }
                #endregion

                public static void ExecRoute(IApiService service)
                {
                        IUserPage page = ApiHelper.GetPage(service.Request.Content, out byte[] content);
                        if (page == null)
                        {
                                throw new ErrorException("web.page.analysis.error");
                        }
                        else if (!_RouteApi.TryGetValue(page.Direct, out ApiHandler handler))
                        {
                                throw new ErrorException("http.404");
                        }
                        else
                        {
                                handler.Execute(service, page, content);
                        }
                }
        }
}
