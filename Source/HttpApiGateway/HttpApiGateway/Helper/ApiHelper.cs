using System;
using System.Reflection;
using System.Text;

using ApiGateway;
using ApiGateway.Attr;

using HttpApiGateway.Interface;
using HttpApiGateway.Model;

using HttpService.Interface;

using RpcHelper;

namespace HttpApiGateway.Helper
{
        internal class ApiHelper
        {
                /// <summary>
                /// 获取授权Id
                /// </summary>
                /// <param name="request"></param>
                /// <returns></returns>
                internal static string GetAccreditId(IHttpRequest request)
                {
                        if (!string.IsNullOrEmpty(request.QueryString["accreditId"]))
                        {
                                return request.QueryString["accreditId"];
                        }
                        else if (!string.IsNullOrEmpty(request.Headers["accreditId"]))
                        {
                                return request.Headers["accreditId"];
                        }
                        return null;
                }
                private static bool _IsBasicType(Type type)
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
                private static string _FormatName(string name)
                {
                        char[] chars = name.ToCharArray();
                        chars[0] = char.ToUpper(chars[0]);
                        return new string(chars);
                }
                private static string _GetIocName(Type type)
                {
                        ApiIocName attr = type.GetCustomAttribute<ApiIocName>();
                        if (attr == null)
                        {
                                return null;
                        }
                        return attr.Name;
                }
                internal static FuncParam GetParamType(ParameterInfo param)
                {
                        if (!param.IsOut)
                        {
                                if (param.ParameterType == ApiPublicDict.IUserStateType || param.ParameterType.GetInterface(ApiPublicDict.IUserStateType.FullName) != null)
                                {
                                        return new FuncParam
                                        {
                                                Name = param.Name.ToLower(),
                                                ParamType = FuncParamType.登陆状态
                                        };
                                }
                                else if (param.ParameterType == ApiPublicDict.IClientIdentity || param.ParameterType.GetInterface(ApiPublicDict.IClientIdentity.FullName) != null)
                                {
                                        return new FuncParam
                                        {
                                                Name = param.Name.ToLower(),
                                                ParamType = FuncParamType.身份标识
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
                                        IsBasicType = isBasic,
                                        ReceiveMethod = isBasic ? "GET" : "POST"
                                };
                        }
                        else if (param.Name == ApiPublicDict.ErrorParamName && param.ParameterType.Name.StartsWith(PublicDataDic.StringTypeName))
                        {
                                return new FuncParam
                                {
                                        Name = param.Name.ToLower(),
                                        ParamType = FuncParamType.错误信息
                                };
                        }
                        else if (param.Name == ApiPublicDict.CountParamName && param.ParameterType.Name.StartsWith(PublicDataDic.LongTypeName))
                        {
                                return new FuncParam
                                {
                                        Name = param.Name.ToLower(),
                                        ParamType = FuncParamType.数据总数
                                };
                        }
                        else
                        {
                                return new FuncParam
                                {
                                        Name = param.Name.ToLower(),
                                        ParamType = FuncParamType.返回值,
                                        DataType = param.ParameterType.GetElementType(),
                                        AttrName = _FormatName(param.Name)
                                };
                        }
                }
                internal static bool InitMethodParam(FuncParam[] param, MethodInfo method, IApiService service, out object[] arg, out string error)
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
                                else if (t.ParamType == FuncParamType.Interface)
                                {
                                        arg[i] = RpcClient.RpcClient.Unity.Resolve(t.DataType, t.IocName);
                                        continue;
                                }
                                else if (t.ParamType != FuncParamType.参数)
                                {
                                        continue;
                                }
                                else
                                {
                                        object obj = _GetMethodParam(t, service);
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
                private static string _GetParamValue(FuncParam param, IApiService service)
                {
                        if (!param.IsBasicType)
                        {
                                return service.Request.PostString;
                        }
                        else if (service.Request.HttpMethod == "POST" && service.Request.ContentType == "application/x-www-form-urlencoded")
                        {
                                return service.Request.Form[param.Name];
                        }
                        else
                        {
                                string str = service.Request.QueryString[param.Name];
                                if (str != null)
                                {
                                        str = Tools.DecodeURI(str);
                                }
                                return str;
                        }
                }

                private static object _GetMethodParam(FuncParam param, IApiService service)
                {
                        string str = _GetParamValue(param, service);
                        if (param.DataType == PublicDataDic.StrType)
                        {
                                return str;
                        }
                        else if (str == null)
                        {
                                return null;
                        }
                        else if (param.IsBasicType)
                        {
                                return _GetValue(str, param.DataType);
                        }
                        return Tools.Json(str, param.DataType);
                }
                private static object _GetValue(string val, Type type)
                {
                        if (type == PublicDataDic.BoolType)
                        {
                                return bool.Parse(val);
                        }
                        else if (type == PublicDataDic.DateTimeType)
                        {
                                return DateTime.Parse(val);
                        }
                        else if (type == PublicDataDic.GuidType)
                        {
                                return Guid.Parse(val);
                        }
                        else if (type == PublicDataDic.IntType)
                        {
                                return int.Parse(val);
                        }
                        else if (type == PublicDataDic.LongType)
                        {
                                return long.Parse(val);
                        }
                        else if (type == PublicDataDic.ShortType)
                        {
                                return short.Parse(val);
                        }
                        else if (type == PublicDataDic.UriType)
                        {
                                return new Uri(val);
                        }
                        else if (type.IsEnum)
                        {
                                return Enum.Parse(type, val);
                        }
                        return Convert.ChangeType(val, type);
                }
                internal static string GetApiUri(RouteConfig config, string name, IApiModular modular)
                {
                        StringBuilder str = new StringBuilder(modular.Config.ApiRouteFormat);
                        str.Replace("{service}", modular.ServiceName);
                        str.Replace("{controller}", _FormatApiName(config.Name));
                        str.Replace("{name}", name);
                        str.Replace("//", "/");
                        return str.ToString();
                }
                internal static string GetApiUri(RouteConfig config, IApiModular modular, string format)
                {
                        if (format.IndexOf('{') != -1)
                        {
                                StringBuilder str = new StringBuilder(format);
                                str.Replace("{service}", modular.ServiceName);
                                str.Replace("{controller}", _FormatApiName(config.Name));
                                str.Replace("//", "/");
                                return str.ToString();
                        }
                        return format;
                }
                private static string _FormatApiName(string name)
                {
                        name = name.ToLower();
                        if (name.EndsWith("api") && name != "api")
                        {
                                name = name.Remove(name.Length - 3);
                        }
                        else if (name.EndsWith("controller"))
                        {
                                name = name.Remove(name.Length - 10);
                        }
                        else if (name.EndsWith("service"))
                        {
                                name = name.Remove(name.Length - 7);
                        }
                        return name;
                }
                public static string GetErrorJson(string error)
                {
                        if (ErrorManage.GetErrorMsg(error, out ErrorMsg msg, out string param))
                        {
                                StringBuilder json = new StringBuilder("{", 150);
                                if (param != null)
                                {
                                        json.AppendFormat("\"errorcode\":{0},\"errmsg\":\"{1}\",\"param\":\"{2}\"", msg.ErrorId, msg.Msg, param);
                                }
                                else
                                {
                                        json.AppendFormat("\"errorcode\":{0},\"errmsg\":\"{1}\"", msg.ErrorId, msg.Msg);
                                }
                                json.Append("}");
                                return json.ToString();
                        }
                        else
                        {
                                return "{\"errorcode\":-1,\"errmsg\":\"" + error + "\"}";
                        }
                }
                public static string GetSuccessJson(object datas, object count)
                {
                        StringBuilder json = new StringBuilder("{\"errorcode\":0,\"data\":{");
                        json.AppendFormat("\"list\":{0},\"count\":{1}", datas.ToJson(), count);
                        json.Append("}}");
                        return json.ToString();
                }
                public static string GetSuccessJson(object model)
                {
                        StringBuilder json = new StringBuilder("{");
                        Type type = model.GetType();
                        if (type.Name == PublicDataDic.BoolType.Name)
                        {
                                json.AppendFormat("\"errorcode\":0,\"data\":{0}", (bool)model ? "true" : "false");
                        }
                        else if (type.Name == PublicDataDic.StringTypeName || type.Name == PublicDataDic.UriTypeName)
                        {
                                json.AppendFormat("\"errorcode\":0,\"data\":\"{0}\"", model);
                        }
                        else if (type.IsPrimitive)
                        {
                                json.AppendFormat("\"errorcode\":0,\"data\":{0}", model);
                        }
                        else
                        {
                                json.AppendFormat("\"errorcode\":0,\"data\":{0}", Tools.Json(model));
                        }
                        json.Append("}");
                        return json.ToString();
                }
        }
}
