using System;
using System.Reflection;

using WeDonekRpc.ApiGateway;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.WebSocketGateway.Interface;
using WeDonekRpc.WebSocketGateway.Model;

namespace WeDonekRpc.WebSocketGateway.Helper
{
    internal class FuncHelper
    {

        public static bool InitMethodParam (FuncParam[] param, bool isForm, MethodInfo method, IWebSocketService service, out object[] arg, out string error)
        {
            arg = new object[param.Length];
            for (int i = 0; i < arg.Length; i++)
            {
                FuncParam t = param[i];
                if (t.ParamType == FuncParamType.登陆状态)
                {
                    arg[i] = service.UserState;
                    continue;
                }
                else if (t.ParamType == FuncParamType.身份标识)
                {
                    arg[i] = service.Identity;
                    continue;
                }
                else if (t.ParamType == FuncParamType.当前请求)
                {
                    arg[i] = service;
                    continue;
                }
                else if (t.ParamType == FuncParamType.请求头)
                {
                    arg[i] = service.Head;
                    continue;
                }
                else if (t.ParamType == FuncParamType.会话)
                {
                    arg[i] = service.Session;
                    continue;
                }
                else if (t.ParamType == FuncParamType.当前模块)
                {
                    arg[i] = service.Modular;
                    continue;
                }
                else if (t.ParamType == FuncParamType.Interface)
                {
                    arg[i] = RpcClient.Ioc.Resolve(t.DataType, t.IocName);
                    continue;
                }
                else if (t.ParamType != FuncParamType.参数)
                {
                    continue;
                }
                else
                {
                    object obj = _GetMethodParam(t, service, isForm);
                    if (!t.IsBasicType && obj == null)
                    {
                        error = string.Format("public.param.null[name={0}]", t.Name);
                        return false;
                    }
                    else
                    {
                        arg[i] = obj;
                    }
                }
            }
            return MethodValidateHelper.ValidateMethod(method, arg, out error);
        }

        public static FuncParam[] InitMethod (MethodInfo method)
        {
            return method.GetParameters().ConvertAll(a => _GetParamType(a));
        }
        private static bool _IsBasicType (Type type)
        {
            if (type.IsPrimitive)
            {
                return type.IsPrimitive;
            }
            else if (type == PublicDataDic.GuidType
                    || type == PublicDataDic.StrType
                    || type == PublicDataDic.UriType
                    || type == PublicDataDic.DateTimeType || type.IsEnum)
            {
                return true;
            }
            return false;
        }
        private static string _GetIocName (Type type)
        {
            ApiIocName attr = type.GetCustomAttribute<ApiIocName>();
            if (attr == null)
            {
                return null;
            }
            return attr.Name;
        }
        private static FuncParam _GetParamType (ParameterInfo param)
        {
            if (param.ParameterType == ApiPublicDict.IUserStateType || param.ParameterType.GetInterface(ApiPublicDict.IUserStateType.FullName) != null)
            {
                return new FuncParam
                {
                    Name = param.Name.ToLower(),
                    ParamType = FuncParamType.登陆状态
                };
            }
            else if (param.ParameterType == ApiPublicDict.IUserIdentity || param.ParameterType.GetInterface(ApiPublicDict.IUserIdentity.FullName) != null)
            {
                return new FuncParam
                {
                    Name = param.Name.ToLower(),
                    ParamType = FuncParamType.身份标识
                };
            }
            else if (param.ParameterType == PublicDict.IApiSocketService)
            {
                return new FuncParam
                {
                    Name = param.Name.ToLower(),
                    ParamType = FuncParamType.当前请求
                };
            }
            else if (param.ParameterType == PublicDict.RequestBody)
            {
                return new FuncParam
                {
                    Name = param.Name.ToLower(),
                    ParamType = FuncParamType.请求头
                };
            }
            else if (param.ParameterType == PublicDict.ISession)
            {
                return new FuncParam
                {
                    Name = param.Name.ToLower(),
                    ParamType = FuncParamType.会话
                };
            }
            else if (param.ParameterType == PublicDict.ICurrentModular)
            {
                return new FuncParam
                {
                    Name = param.Name.ToLower(),
                    ParamType = FuncParamType.当前模块
                };
            }
            else if (param.ParameterType.IsInterface)
            {
                return new FuncParam
                {
                    Name = param.Name.ToLower(),
                    ParamType = FuncParamType.Interface,
                    DataType = param.ParameterType,
                    IocName = _GetIocName(param.ParameterType)
                };
            }
            bool isBasic = _IsBasicType(param.ParameterType);
            return new FuncParam
            {
                Name = param.Name.ToLower(),
                ParamType = FuncParamType.参数,
                DataType = param.ParameterType,
                IsBasicType = isBasic
            };
        }


        private static object _GetMethodParam (FuncParam param, IWebSocketService service, bool isForm)
        {
            if (isForm)
            {
                string str = service.Form[param.Name];
                return _GetMethodParam(str, param, service);
            }
            return _GetMethodParam(service.PostString, param, service);
        }
        private static object _GetMethodParam (string str, FuncParam param, IWebSocketService service)
        {
            if (param.DataType == PublicDataDic.StrType)
            {
                return str;
            }
            else if (str.IsNull())
            {
                return null;
            }
            else if (param.IsBasicType)
            {
                return str.Parse(param.DataType);
            }
            return str.Json(param.DataType);
        }
    }
}
