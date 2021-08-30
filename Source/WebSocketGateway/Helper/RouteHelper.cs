using System.Linq;
using System.Reflection;

using ApiGateway;
using ApiGateway.Attr;

using RpcHelper;

using WebSocketGateway.Interface;
using WebSocketGateway.Model;

using WebSocketGateway.Route;

namespace WebSocketGateway.Helper
{
        internal class RouteHelper
        { 
                public static bool GetApi(ApiModel model, out ISocketApi api)
                {
                        MethodInfo method = model.Method;
                        if (_CheckOutFuc(method))
                        {
                                api = new OutFuncApi(model);
                        }
                        else
                        {
                                api = new FuncApi(model);
                        }
                        return api.VerificationApi();
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
               
                public static ApiModel GetApiParam(MethodInfo method, IRouteConfig config, IApiModular modular)
                {
                        ApiRouteName rname = (ApiRouteName)method.GetCustomAttribute(ApiPublicDict.ApiRouteName);
                        ApiModel model = new ApiModel
                        {
                                LocalPath = rname == null ? ApiHelper.GetApiPath(config, method.Name, modular) : rname.FormatPath(config, modular),
                                Method = method,
                                Type = config.Type,
                                Prower = config.Prower,
                                IsAccredit = config.IsAccredit
                        };
                        ApiPrower attr = (ApiPrower)method.GetCustomAttribute(ApiPublicDict.ProwerAttr);
                        if (attr != null)
                        {
                                model.IsAccredit = attr.IsAccredit;
                                model.Prower = attr.Prower;
                        }
                        return model;
                }
        }
}
