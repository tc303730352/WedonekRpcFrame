using System.Collections.Generic;
using System.Reflection;

using HttpApiGateway.Model;

using RpcHelper;

namespace HttpApiGateway.Helper
{
        internal class FuncHelper
        {
                public static object GetReturns(object result, int[] outs, object[] param, FuncParam[] funcArg)
                {
                        if (outs.Length == 0)
                        {
                                return result;
                        }
                        Dictionary<string, object> obj = new Dictionary<string, object>
                        {
                                { "Returns", result }
                        };
                        outs.ForEach(a =>
                        {
                                FuncParam p = funcArg[a];
                                obj.Add(p.AttrName, param[a]);
                        });
                        return obj;
                }
                public static FuncParam[] InitMethod(MethodInfo method)
                {
                        List<int> rets = new List<int>();
                        FuncParam[] list = method.GetParameters().ConvertAll(a => ApiHelper.GetParamType(a));
                        if (list.IsExists(b => b.ParamType == FuncParamType.参数 && !b.IsBasicType))
                        {
                                list.ForEach(a => a.ParamType == FuncParamType.参数 && a.IsBasicType, a =>
                                {
                                        a.ReceiveMethod = "GET";
                                });
                        }
                        return list;
                }
        }
}
