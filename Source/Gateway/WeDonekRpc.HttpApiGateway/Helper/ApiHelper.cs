using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;
using System.Xml;
using WeDonekRpc.ApiGateway;
using WeDonekRpc.ApiGateway.Attr;
using WeDonekRpc.ApiGateway.Helper;
using WeDonekRpc.ApiGateway.Json;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Error;
using WeDonekRpc.Helper.Json;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpApiGateway.Model;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpApiGateway.Helper
{
    internal class ApiHelper
    {
        private static readonly Type _XmlType = typeof(XmlDocument);
        /// <summary>
        /// 获取授权Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal static string GetAccreditId (IHttpRequest request)
        {
            if (request.QueryString["accreditId"].IsNotNull())
            {
                return request.QueryString["accreditId"];
            }
            else if (request.Headers["accreditId"].IsNotNull())
            {
                return request.Headers["accreditId"];
            }
            return null;
        }
        private static bool _IsBasicType (Type type)
        {
            if (type.Name == PublicDataDic.Nullable)
            {
                type = type.GenericTypeArguments[0];
            }
            if (type.IsPrimitive)
            {
                return type.IsPrimitive;
            }
            else if (type == PublicDataDic.GuidType
                    || type == PublicDataDic.StrType
                    || type == PublicDataDic.UriType
                    || type == PublicDataDic.DateTimeType
                    || type.IsEnum)
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
        internal static FuncParam GetParamType (ParameterInfo param)
        {
            FuncParam t = _GetParamType(param);
            if (t.ParamType == FuncParamType.参数)
            {
                string name = t.Name;
                t.ReceiveMethod = ApiParamHelper.GetReceiveMethod(param, t.ReceiveMethod, ref name);
                t.Name = name;
            }
            return t;
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
            else if (param.ParameterType == _XmlType)
            {
                return new FuncParam
                {
                    Name = param.Name.ToLower(),
                    ParamType = FuncParamType.XML,
                    ReceiveMethod = "POST"
                };
            }
            else if (param.ParameterType == typeof(IApiService))
            {
                return new FuncParam
                {
                    ParamType = FuncParamType.当前请求,
                    Name = param.Name.ToLower(),
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
                DefValue = isBasic ? Tools.GetTypeDefValue(param.ParameterType) : null,
                ReceiveMethod = isBasic ? "GET" : "POST"
            };
        }
        internal static bool InitMethodParam (FuncParam[] param, MethodInfo method, IApiService service, out object[] arg, out string error)
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
                else if (t.ParamType == FuncParamType.当前请求)
                {
                    arg[i] = service;
                }
                else if (t.ParamType == FuncParamType.身份标识)
                {
                    arg[i] = service.Identity;
                    continue;
                }
                else if (t.ParamType == FuncParamType.XML)
                {
                    string xml = service.Request.PostString;
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);
                    arg[i] = doc;
                    continue;
                }
                else if (t.ParamType == FuncParamType.Interface)
                {
                    if (t.IocName == null)
                    {
                        arg[i] = RpcClient.Ioc.Resolve(t.DataType);
                    }
                    else
                    {
                        arg[i] = RpcClient.Ioc.Resolve(t.DataType, t.IocName);
                    }
                    continue;
                }
                else if (t.ParamType != FuncParamType.参数)
                {
                    continue;
                }
                else
                {
                    dynamic obj = _GetMethodParam(t, service);
                    if (t.IsBasicType && obj == null)
                    {
                        arg[i] = t.DefValue;
                    }
                    else
                    {
                        arg[i] = obj;
                    }
                }
            }
            return MethodValidateHelper.ValidateMethod(method, arg, out error);
        }
        private static string _GetJsonStr (NameValueCollection body)
        {
            StringBuilder json = new StringBuilder("{");
            foreach (string i in body.Keys)
            {
                _ = json.AppendFormat("\"{0}\":\"{1}\"", i, body[i]);
            }
            _ = json.Append("}");
            return json.ToString();
        }
        private static string _GetParamValue (FuncParam param, IApiService service)
        {
            if (param.ReceiveMethod == "POST" && service.Request.ContentType == "application/json")
            {
                return service.Request.PostString;
            }
            else if (service.Request.HttpMethod == "POST" && service.Request.ContentType == "application/x-www-form-urlencoded")
            {
                if (param.IsBasicType)
                {
                    return service.Request.Form[param.Name];
                }
                else
                {
                    return _GetJsonStr(service.Request.Form);
                }
            }
            else if (param.ReceiveMethod == "GET")
            {
                if (param.IsBasicType)
                {
                    string str = service.Request.QueryString[param.Name];
                    if (str != null)
                    {
                        str = Tools.DecodeURI(str);
                    }
                    return str;
                }
                else
                {
                    return _GetJsonStr(service.Request.QueryString);
                }
            }
            else if (param.ReceiveMethod == "Head")
            {
                if (param.IsBasicType)
                {
                    return service.Request.Headers[param.Name];
                }
                else
                {
                    return _GetJsonStr(service.Request.Headers);
                }
            }
            else if (param.ReceiveMethod == "PostForm")
            {
                if (param.IsBasicType)
                {
                    return service.Request.Form[param.Name];
                }
                else
                {
                    return _GetJsonStr(service.Request.Form);
                }
            }
            else if (param.ReceiveMethod == "Route")
            {
                if (service.RouteArgs.TryGetValue(param.Name, out string str))
                {
                    return str;
                }
                return null;
            }
            else if (param.ReceiveMethod == "Path")
            {
                if (service.PathArgs.TryGetValue(param.Name, out string str))
                {
                    return str;
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        private static dynamic _GetMethodParam (FuncParam param, IApiService service)
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
                if (!str.TryParse(param.DataType, out dynamic res))
                {
                    throw new ErrorException("gateway.parameter.type.error[param=" + param.Name + "]");
                }
                return res;
            }
            return GatewayJsonTools.Json(str, param.DataType);
        }


        public static string GetErrorJson (string error)
        {
            if (LocalErrorManage.GetErrorMsg(error, out ErrorMsg msg, out string param))
            {
                StringBuilder json = new StringBuilder("{", 150);
                if (param != null)
                {
                    _ = json.AppendFormat("\"errorcode\":{0},\"errmsg\":\"{1}\",\"param\":\"{2}\"", msg.ErrorId, msg.Text, param);
                }
                else
                {
                    _ = json.AppendFormat("\"errorcode\":{0},\"errmsg\":\"{1}\"", msg.ErrorId, msg.Text);
                }
                _ = json.Append("}");
                return json.ToString();
            }
            else
            {
                return "{\"errorcode\":-1,\"errmsg\":\"" + error + "\"}";
            }
        }

        public static string GetSuccessJson (object model)
        {
            StringBuilder json = new StringBuilder("{");
            Type type = model.GetType();
            if (type.Name == PublicDataDic.BoolType.Name)
            {
                _ = json.AppendFormat("\"errorcode\":0,\"data\":{0}", (bool)model ? "true" : "false");
            }
            else if (type.Name == PublicDataDic.StringTypeName || type.Name == PublicDataDic.UriTypeName)
            {
                _ = json.AppendFormat("\"errorcode\":0,\"data\":\"{0}\"", model);
            }
            else if (type.IsPrimitive)
            {
                _ = json.AppendFormat("\"errorcode\":0,\"data\":{0}", model);
            }
            else
            {
                _ = json.AppendFormat("\"errorcode\":0,\"data\":{0}", GatewayJsonTools.Json(model));
            }
            _ = json.Append("}");
            return json.ToString();
        }
    }
}
