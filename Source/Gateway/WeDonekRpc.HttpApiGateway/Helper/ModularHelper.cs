using System;
using System.Reflection;
using System.Text;
using WeDonekRpc.ApiGateway;
using WeDonekRpc.HttpApiGateway.FileUp;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;
using WeDonekRpc.HttpApiGateway.Route;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.FileUp.Model;

namespace WeDonekRpc.HttpApiGateway.Helper
{
    internal class ModularHelper
    {
        private static readonly Type _apiBasicType = typeof(ApiController);

        public static ApiRouteBuffer GetApiRoute ( MethodInfo method, IApiModular modular, RouteSet routeSet )
        {
            if ( _TryAddApiType(method, method.DeclaringType, modular, routeSet, out ApiRouteBuffer route) )
            {
                return route;
            }
            else if ( _TryAddGateway(method, method.DeclaringType, modular, routeSet, out route) )
            {
                return route;
            }
            else if ( _TryRegStaticRoute(method, modular, routeSet, out route) )
            {
                return route;
            }
            else
            {
                throw new ErrorException("rpc.gateway.interface.no.find");
            }
        }

        public static ApiRouteBuffer[] GetApiBody ( Type[] types, IApiModular modular )
        {
            return types.Convert(a =>
            {
                if ( !a.IsClass || a == _apiBasicType || a.IsIgnore() )
                {
                    return null;
                }
                else if ( _RegisterApi(a, modular, out ApiRouteBuffer body) )
                {
                    return body;
                }
                else if ( _UpBlockFileApi(a, modular, out body) )
                {
                    return body;
                }
                else if ( _RegisterGateway(a, modular, out body) )
                {
                    return body;
                }
                else
                {
                    return null;
                }
            });
        }

        #region 注册网关或API路由
        private static bool _TryRegStaticRoute ( MethodInfo method, IApiModular modular, RouteSet routeSet, out ApiRouteBuffer route )
        {
            if ( !method.IsStatic )
            {
                route = null;
                return false;
            }
            IRouteConfig config = ApiRouteHelper.GetDefRouteConfig(method.DeclaringType, modular, routeSet);
            route = new ApiRouteBuffer
            {
                Config = config
            };
            _RegApi(method, route, modular);
            return true;
        }
        private static bool _RegisterGateway ( Type type, IApiModular modular, out ApiRouteBuffer body )
        {
            Type apiType = type.GetInterface(PublicDict.IGatewayType.FullName);
            if ( apiType == null )
            {
                body = null;
                return false; ;
            }
            IRouteConfig config = ApiRouteHelper.GetDefRouteConfig(type, modular);
            MethodInfo[] methods = type.GetMethods();
            ApiRouteBuffer buffer = new ApiRouteBuffer
            {
                Form = PublicDict.IGatewayType,
                To = type,
                Name = type.FullName,
                Config = config,
            };
            if ( methods.Length > 0 )
            {
                string[] pros = type.GetProperties().ConvertAll(a => a.Name);
                methods.ForEach(c =>
                {
                    _RegApi(c, buffer, modular, pros);
                });
            }
            body = buffer;
            return true;
        }
        private static void _RegApi ( MethodInfo method, ApiRouteBuffer buffer, IApiModular modular, string[] pros )
        {
            if ( method.DeclaringType == PublicDict.IApiType
                || method.DeclaringType == PublicDataDic.ObjectType
                || method.GetCustomAttribute(ApiPublicDict.ApiMethodIgnore) != null )
            {
                return;
            }
            else if ( !method.IsPublic || method.IsStatic || method.IsAbstract )
            {
                return;
            }
            else if ( ( method.Name.StartsWith("get_") || method.Name.StartsWith("set_") ) && pros.IsExists(a => method.Name.EndsWith(a)) )
            {
                return;
            }
            _RegApi(method, buffer, modular);
        }
        private static void _RegApi ( MethodInfo method, ApiRouteBuffer buffer, IApiModular modular )
        {
            ApiModel model = ApiRouteHelper.GetApiParam(method, buffer.Config, modular);
            ApiType type = model.ApiType;
            IHttpApi api = ApiRouteHelper.GetApi(model, ref type);
            model.ApiType = type;
            modular.InitRouteConfig(model);
            IApiRoute route = new ApiRoute(api, model, modular);
            buffer.Apis.Add(new ApiBody
            {
                ApiEventType = model.ApiEventType,
                Name = route.ApiEventType,
                Route = route,
                UpConfigType = model.UpConfig
            });
        }
        private static bool _RegisterApi ( Type type, IApiModular modular, out ApiRouteBuffer body )
        {
            Type apiType = type.GetInterface(PublicDict.IApiType.FullName);
            if ( apiType == null )
            {
                body = null;
                return false; ;
            }
            IRouteConfig config = ApiRouteHelper.GetDefRouteConfig(type, modular);
            MethodInfo[] methods = type.GetMethods();
            ApiRouteBuffer buffer = new ApiRouteBuffer
            {
                Form = PublicDict.IGatewayType,
                To = type,
                Name = type.FullName,
                Config = config,
            };
            if ( methods.Length > 0 )
            {
                string[] pros = type.GetProperties().ConvertAll(a => a.Name);
                methods.ForEach(c =>
                {
                    _RegApi(c, buffer, modular, pros);
                });
            }
            body = buffer;
            return true;
        }
        private static bool _UpBlockFileApi ( Type type, IApiModular modular, out ApiRouteBuffer body )
        {
            Type apiType = type.GetInterface(PublicDict.IUpBlockFileType.FullName);
            if ( apiType == null )
            {
                body = null;
                return false;
            }
            IRouteConfig config = ApiRouteHelper.GetDefRouteConfig(type, modular);
            ApiModel model = ApiRouteHelper.GetApiParam(config, modular);
            Type argType = null;
            if ( !config.Type.BaseType.GenericTypeArguments.IsNull() )
            {
                Type t = typeof(UpFileData<>);
                argType = t.MakeGenericType(config.Type.BaseType.GenericTypeArguments[0]);
            }
            else
            {
                apiType = typeof(UpFileData);
            }
            IHttpApi api = new BlockUpApi(model, new FuncParam
            {
                ParamType = FuncParamType.参数,
                AttrName = "data",
                DataType = argType,
                IsBasicType = false,
                Name = "File",
                ReceiveMethod = "POST"
            });
            body = new ApiRouteBuffer
            {
                Form = PublicDict.IUpBlockFileType,
                To = type,
                Name = type.FullName,
                Config = config,
            };
            IApiRoute route = new ApiRoute(api, model, modular);
            body.Apis.Add(new ApiBody
            {
                ApiEventType = model.ApiEventType,
                Name = route.ApiEventType,
                Route = route,
                UpConfigType = model.UpConfig
            });
            return true;
        }
        private static bool _RegApiRoute ( Type form, MethodInfo method, Type type, IApiModular modular, RouteSet routeSet, out ApiRouteBuffer route )
        {
            IRouteConfig config = ApiRouteHelper.GetDefRouteConfig(type, modular, routeSet);
            route = new ApiRouteBuffer
            {
                Form = form,
                To = type,
                Name = type.FullName,
                Config = config
            };
            string[] pros = type.GetProperties().ConvertAll(a => a.Name);
            _RegApi(method, route, modular, pros);
            return true;
        }
        private static bool _TryAddGateway ( MethodInfo method, Type type, IApiModular modular, RouteSet routeSet, out ApiRouteBuffer route )
        {
            if ( type.GetInterface(PublicDict.IGatewayType.FullName) == null )
            {
                route = null;
                return false;
            }
            return _RegApiRoute(PublicDict.IGatewayType, method, type, modular, routeSet, out route);
        }
        private static bool _TryAddApiType ( MethodInfo method, Type type, IApiModular modular, RouteSet routeSet, out ApiRouteBuffer route )
        {
            if ( type.GetInterface(PublicDict.IApiType.FullName) == null )
            {
                route = null;
                return false;
            }
            return _RegApiRoute(PublicDict.IGatewayType, method, type, modular, routeSet, out route);
        }
        #endregion
    }
}
