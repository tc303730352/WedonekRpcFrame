using System;
using System.Collections.Concurrent;
using System.Reflection;

using ApiGateway;
using ApiGateway.Attr;

using HttpApiGateway.ActionFun;
using HttpApiGateway.Helper;
using HttpApiGateway.Interface;
using HttpApiGateway.Model;

using RpcHelper;

namespace HttpApiGateway.Collect
{
        internal class ApiRouteCollect
        {
                private static readonly ConcurrentDictionary<string, IApiRoute> _RouteApi = new ConcurrentDictionary<string, IApiRoute>();
                #region 注册
                private static RouteConfig _RegApi(Type type, IApiModular modular, Action<IRoute> action)
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
                        ApiGatewayService.RegModular(modular.ServiceName, type);
                        MethodInfo[] methods = type.GetMethods();
                        methods.ForEach(a =>
                        {
                                if (a.IsPublic && a.DeclaringType == type)
                                {
                                        _RegApi(a, config, modular, action);
                                }
                        });
                        HttpService.HttpService.Sort();
                        return config;
                }
                public static void RegApi(Type type, IApiModular modular, Action<IRoute> action)
                {
                        _RegApi(type, modular, action);
                }
                public static void RegApi(Type type, IApiController api, IApiModular modular, Action<IRoute> action)
                {
                        RouteConfig config = _RegApi(type, modular, action);
                        api?.Install(config);
                }
                private static ApiModel _GetApiParam(MethodInfo method, RouteConfig config, IApiModular modular)
                {
                        ApiRouteName rname = (ApiRouteName)method.GetCustomAttribute(ApiPublicDict.ApiRouteName);
                        ApiModel model = new ApiModel
                        {
                                ApiUri = rname == null ? ApiHelper.GetApiUri(config, method.Name, modular) : rname.FormatPath(config, modular),
                                Method = method,
                                Type = config.Type,
                                Prower = config.Prower,
                                IsAccredit = config.IsAccredit,
                                ApiType = ApiType.API接口
                        };
                        ApiPrower attr = (ApiPrower)method.GetCustomAttribute(ApiPublicDict.ProwerAttr);
                        if (attr != null)
                        {
                                model.IsAccredit = attr.IsAccredit;
                                model.Prower = attr.Prower;
                        }
                        ApiUpConfig upConfig = (ApiUpConfig)method.GetCustomAttribute(ApiPublicDict.ApiUpConfig);
                        if (upConfig != null)
                        {
                                model.UpCheck = upConfig;
                                model.ApiType = ApiType.文件上传;
                        }
                        else
                        {
                                model.UpCheck = ApiGatewayService.Config.UpConfig;
                        }
                        return model;
                }
                private static void _RegApi(MethodInfo method, RouteConfig config, IApiModular modular, Action<IRoute> action)
                {
                        ApiModel model = _GetApiParam(method, config, modular);
                        ApiType type = model.ApiType;
                        if (_GetApi(model, out IHttpApi api, ref type))
                        {
                                model.ApiType = type;
                                IApiRoute route = new ApiRoute(api, model, config, modular);
                                if (_RouteApi.TryAdd(api.ApiName, route))
                                {
                                        route.InitRoute();
                                        action(route);
                                }
                        }
                }
                private static bool _CheckOutFuc(MethodInfo method)
                {
                        if (method.ReturnType != PublicDataDic.BoolType)
                        {
                                return false;
                        }
                        ParameterInfo[] param = method.GetParameters();
                        if (param.Length < 1 || param.Count(a => a.IsOut) < 1)
                        {
                                return false;
                        }
                        return true;
                }
                private static bool _CheckIsResponseFun(MethodInfo method)
                {
                        Type type = method.ReturnType;
                        if (type == PublicDataDic.VoidType)
                        {
                                return false;
                        }
                        else if (type == PublicDict.ResponseType)
                        {
                                return true;
                        }
                        return type.GetInterface(PublicDict.ResponseType.FullName) != null;
                }
                private static bool _GetApi(ApiModel model, out IHttpApi api, ref ApiType type)
                {
                        MethodInfo method = model.Method;
                        if (_CheckOutFuc(method))
                        {
                                api = new OutFuncApi(model);
                        }
                        else if (_CheckIsResponseFun(method))
                        {
                                api = new ResponseApi(model);
                                type = ApiType.数据流;
                        }
                        else
                        {
                                api = new FuncApi(model);
                        }
                        return api.VerificationApi();
                }
                #endregion


        }
}
