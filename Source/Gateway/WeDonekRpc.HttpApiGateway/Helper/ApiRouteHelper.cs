using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using WeDonekRpc.ApiGateway;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.ActionFun;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;

namespace WeDonekRpc.HttpApiGateway.Helper
{
    internal delegate void ExecAction (object target, object[] args);
    internal delegate object ExecFunc (object target, object[] args);

    internal delegate IResponse ExecResponse (object targer, object[] args);

    internal delegate Task ExecTaskAction (object target, object[] args);

    internal class ApiRouteHelper
    {

        public static ExecTaskAction GetExecTaskAction (MethodInfo method)
        {
            ParameterExpression target = Expression.Parameter(PublicDataDic.ObjectType, "target");
            ParameterExpression arrays = Expression.Parameter(typeof(object[]), "args");
            ParameterInfo[] parameters = method.GetParameters();
            Expression[] list = new Expression[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                list[i] = Expression.Convert(Expression.ArrayIndex(arrays, Expression.Constant(i)), parameters[i].ParameterType);
            }
            if (method.IsStatic)
            {
                MethodCallExpression methodExp = Expression.Call(method, list);
                return Expression.Lambda<ExecTaskAction>(methodExp, target, arrays).Compile();
            }
            else
            {
                Expression exception = Expression.Convert(target, method.DeclaringType);
                MethodCallExpression methodExp = Expression.Call(exception, method, list);
                return Expression.Lambda<ExecTaskAction>(methodExp, target, arrays).Compile();
            }
        }
        public static ExecResponse GetExecResponse (MethodInfo method)
        {
            ParameterExpression target = Expression.Parameter(PublicDataDic.ObjectType, "target");
            ParameterExpression arrays = Expression.Parameter(typeof(object[]), "args");
            ParameterInfo[] parameters = method.GetParameters();
            Expression[] list = new Expression[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                list[i] = Expression.Convert(Expression.ArrayIndex(arrays, Expression.Constant(i)), parameters[i].ParameterType);
            }
            if (method.IsStatic)
            {
                MethodCallExpression methodExp = Expression.Call(method, list);
                return Expression.Lambda<ExecResponse>(Expression.Convert(methodExp, PublicDict.ResponseType), target, arrays).Compile();
            }
            else
            {
                Expression exception = Expression.Convert(target, method.DeclaringType);
                MethodCallExpression methodExp = Expression.Call(exception, method, list);
                return Expression.Lambda<ExecResponse>(Expression.Convert(methodExp, PublicDict.ResponseType), target, arrays).Compile();
            }
        }
        public static ExecAction GetExecAction (MethodInfo method)
        {
            ParameterExpression target = Expression.Parameter(PublicDataDic.ObjectType, "target");
            ParameterExpression arrays = Expression.Parameter(typeof(object[]), "args");
            ParameterInfo[] parameters = method.GetParameters();
            Expression[] list = new Expression[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                list[i] = Expression.Convert(Expression.ArrayIndex(arrays, Expression.Constant(i)), parameters[i].ParameterType);
            }
            if (method.IsStatic)
            {
                MethodCallExpression methodExp = Expression.Call(method, list);
                return Expression.Lambda<ExecAction>(methodExp, target, arrays).Compile();
            }
            else
            {
                Expression exception = Expression.Convert(target, method.DeclaringType);
                MethodCallExpression methodExp = Expression.Call(exception, method, list);
                return Expression.Lambda<ExecAction>(methodExp, target, arrays).Compile();
            }
        }
        public static ExecFunc GetExecFunc (MethodInfo method)
        {
            ParameterExpression target = Expression.Parameter(PublicDataDic.ObjectType, "target");
            ParameterExpression arrays = Expression.Parameter(typeof(object[]), "args");
            ParameterInfo[] parameters = method.GetParameters();
            Expression[] list = new Expression[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                list[i] = Expression.Convert(Expression.ArrayIndex(arrays, Expression.Constant(i)), parameters[i].ParameterType);
            }
            if (method.IsStatic)
            {
                MethodCallExpression methodExp = Expression.Call(method, list);
                return Expression.Lambda<ExecFunc>(Expression.Convert(methodExp, PublicDataDic.ObjectType), target, arrays).Compile();
            }
            else
            {
                Expression exception = Expression.Convert(target, method.DeclaringType);
                MethodCallExpression methodExp = Expression.Call(exception, method, list);
                return Expression.Lambda<ExecFunc>(Expression.Convert(methodExp, PublicDataDic.ObjectType), target, arrays).Compile();
            }
        }

        private static bool _CheckIsResponseFun (MethodInfo method, out bool isTask)
        {
            Type type = method.ReturnType;
            if (type.Name == PublicDataDic.TaskTName)
            {
                isTask = true;
                type = type.GetGenericArguments()[0];
            }
            else
            {
                isTask = false;
            }
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
        public static bool CheckMethod (MethodInfo method)
        {
            ParameterInfo[] param = method.GetParameters();
            if (param.Length == 0)
            {
                return true;
            }
            return !param.IsExists(a => a.IsOut || a.ParameterType.IsByRef);
        }
        public static IHttpApi GetApi (ApiModel model, ref ApiType type)
        {
            MethodInfo method = model.Method;
            if (_CheckIsResponseFun(method, out bool isTask))
            {
                type = ApiType.数据流;
                if (isTask)
                {
                    return new TaskResponseApi(model);
                }
                return new ResponseApi(model);
            }
            else if (method.ReturnType == PublicDataDic.TaskType)
            {
                return new VoidTaskFuncApi(model);
            }
            else if (method.ReturnType == PublicDataDic.VoidType)
            {
                return new VoidFuncApi(model);
            }
            else if (method.ReturnType.Name == PublicDataDic.TaskTName)
            {
                return new ReturnTaskFuncApi(model);
            }
            else
            {
                return new ReturnFuncApi(model);
            }
        }
        public static bool _CheckIsEnable (MethodInfo method)
        {
            return method.GetCustomAttribute(ApiPublicDict.ApiStop) == null;
        }
        public static bool? _CheckIsEnable (Type type)
        {
            if (type.GetCustomAttribute(ApiPublicDict.ApiStop) == null)
            {
                return null;
            }
            return false;
        }
        public static ApiModel GetApiParam (MethodInfo method, IRouteConfig config, IApiModular modular)
        {
            ApiModel model = new ApiModel
            {
                Method = method,
                Type = config.Type,
                Prower = config.Prower,
                IsAccredit = config.IsAccredit,
                ApiType = ApiType.API接口,
                ApiUri = ApiGateway.Helper.ApiHelper.GetApiPath(config, modular, method, out bool isRegex),
                IsRegex = isRegex
            };
            ApiEventAttr serviceEvent = (ApiEventAttr)method.GetCustomAttribute(ApiPublicDict.ApiEventAttrName);
            if (serviceEvent == null)
            {
                model.ApiEventType = config.ApiEventType;
            }
            else
            {
                model.ApiEventType = serviceEvent.ServiceEventType;
            }
            if (!config.IsEnable.HasValue)
            {
                model.IsEnable = _CheckIsEnable(method);
            }
            else
            {
                model.IsEnable = config.IsEnable.Value;
            }
            ApiPrower attr = (ApiPrower)method.GetCustomAttribute(ApiPublicDict.ProwerAttr);
            if (attr != null)
            {
                model.IsAccredit = attr.IsAccredit;
                model.Prower = attr.Prower;
            }
            ApiUpConfig upConfig = (ApiUpConfig)method.GetCustomAttribute(ApiPublicDict.ApiUpConfig);
            if (upConfig != null)
            {
                model.UpConfig = upConfig.ConfigType;
                model.UpSet = upConfig.ToUpSet();
                model.ApiType = ApiType.文件上传;
            }
            else if (config.UpConfig != null || config.UpSet != null)
            {
                model.ApiType = ApiType.文件上传;
                model.UpConfig = config.UpConfig;
                model.UpSet = config.UpSet;
            }
            else
            {
                model.UpSet = ApiGatewayService.Config.UpConfig.ToUpSet();
            }
            return model;
        }
        public static ApiModel GetApiParam (IRouteConfig config, IApiModular modular)
        {
            ApiModel model = new ApiModel
            {
                Type = config.Type,
                Prower = config.Prower,
                IsAccredit = config.IsAccredit,
                ApiEventType = config.ApiEventType,
                IsEnable = config.IsEnable.GetValueOrDefault(true),
                ApiType = ApiType.文件上传,
                ApiUri = ApiGateway.Helper.ApiHelper.GetApiPath(config, modular, out bool isRegex),
                IsRegex = isRegex,
                UpConfig = config.UpConfig,
                UpSet = config.UpSet
            };
            if (config.UpSet == null && config.UpConfig == null)
            {
                model.UpSet = ApiGatewayService.Config.UpConfig.ToUpSet();
            }
            return model;
        }
        public static IRouteConfig GetDefRouteConfig (Type type, IApiModular modular)
        {
            ApiPrower prower = (ApiPrower)type.GetCustomAttribute(ApiPublicDict.ProwerAttr);
            ApiRouteName route = (ApiRouteName)type.GetCustomAttribute(ApiPublicDict.ApiRouteName);
            ApiEventAttr serviceEvent = (ApiEventAttr)type.GetCustomAttribute(ApiPublicDict.ApiEventAttrName);
            RouteConfig config = new RouteConfig
            {
                Type = type,
                Route = route,
                IsAccredit = true,
                IsEnable = _CheckIsEnable(type),
                ApiEventType = serviceEvent?.ServiceEventType
            };
            if (prower != null)
            {
                config.IsAccredit = prower.IsAccredit;
                config.Prower = prower.Prower;
            }
            else
            {
                config.IsAccredit = modular.Config.IsAccredit;
            }
            _InitUpConfig(type, config);
            return config;
        }
        private static void _InitUpConfig (Type type, RouteConfig config)
        {
            ApiUpConfig upConfig = (ApiUpConfig)type.GetCustomAttribute(ApiPublicDict.ApiUpConfig);
            if (upConfig != null)
            {
                config.UpConfig = upConfig.ConfigType;
                config.UpSet = upConfig.ToUpSet();
            }
            else if (config.UpConfig != null)
            {
                config.UpConfig = config.UpConfig;
                config.UpSet = config.UpSet;
            }
        }
        public static IRouteConfig GetDefRouteConfig (Type type, IApiModular modular, RouteSet routeSet)
        {
            if (routeSet == null)
            {
                return GetDefRouteConfig(type, modular);
            }
            RouteConfig config = new RouteConfig
            {
                Type = type,
                IsAccredit = true
            };
            if (routeSet.IsEnable.HasValue)
            {
                config.IsEnable = routeSet.IsEnable.Value;
            }
            else
            {
                config.IsEnable = _CheckIsEnable(type);
            }
            if (routeSet.IsAccredit.HasValue)
            {
                config.IsAccredit = routeSet.IsAccredit.Value;
                config.Prower = routeSet.Prower;
            }
            else
            {
                ApiPrower prower = (ApiPrower)type.GetCustomAttribute(ApiPublicDict.ProwerAttr);
                if (prower != null)
                {
                    config.IsAccredit = prower.IsAccredit;
                    config.Prower = prower.Prower;
                }
                else
                {
                    config.IsAccredit = modular.Config.IsAccredit;
                }
            }
            if (!routeSet.RoutePath.IsNull())
            {
                config.Route = new ApiRouteName(routeSet.RoutePath, routeSet.IsRegex);
            }
            else
            {
                config.Route = (ApiRouteName)type.GetCustomAttribute(ApiPublicDict.ApiRouteName);
            }
            if (routeSet.ApiEventType != null)
            {
                config.ApiEventType = routeSet.ApiEventType;
            }
            else
            {
                ApiEventAttr serviceEvent = (ApiEventAttr)type.GetCustomAttribute(ApiPublicDict.ApiEventAttrName);
                config.ApiEventType = serviceEvent?.ServiceEventType;
            }
            if (routeSet.UpSet == null)
            {
                _InitUpConfig(type, config);
            }
            else
            {
                config.UpSet = routeSet.UpSet;
            }
            return config;
        }
        public static string GetMethodKey (MethodInfo method)
        {
            return string.Concat(method.DeclaringType.FullName, ".", method.ToString()).GetMd5();
        }
    }
}
