using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using RpcHelper.Validate;

namespace RpcHelper
{
        public class MethodValidateHelper
        {
                private static readonly ConcurrentDictionary<long, MethodValidateCache> _ValidateCache = new ConcurrentDictionary<long, MethodValidateCache>();

                internal static Dictionary<int, IValidateData> GetValidateList(MethodInfo method)
                {
                        Dictionary<int, IValidateData> attrs = new Dictionary<int, IValidateData>();
                        ParameterInfo[] param = method.GetParameters().FindAll(a => !a.IsOut && !a.ParameterType.Name.EndsWith("&"));
                        if (param.Length == 0)
                        {
                                return attrs;
                        }
                        param.ForEach(a =>
                        {
                                IValidateAttr[] attr = a.GetCustomAttributes<ValidateAttr>(false).ToArray();
                                if (DataValidateHepler.CheckIsValidate(a.ParameterType))
                                {
                                        attrs.Add(a.Position, new ValidateAgent(a, attr));
                                }
                                else if (attr.Length > 0)
                                {
                                        attrs.Add(a.Position, new ValidateParameter(a, attr));
                                }
                        });
                        return attrs;
                }
                public static bool CheckIsValidate(MethodInfo method)
                {
                        long id = method.MethodHandle.Value.ToInt64();
                        if (!_ValidateCache.TryGetValue(id, out MethodValidateCache cache))
                        {
                                cache = _ValidateCache.GetOrAdd(id, new MethodValidateCache(method));
                                cache.InitValidate();
                        }
                        return cache.AttrNum != 0;
                }
                public static bool CheckIsValidate(MethodInfo method, int index, out IValidateAttr[] attrs)
                {
                        long id = method.MethodHandle.Value.ToInt64();
                        if (!_ValidateCache.TryGetValue(id, out MethodValidateCache cache))
                        {
                                cache = _ValidateCache.GetOrAdd(id, new MethodValidateCache(method));
                                cache.InitValidate();
                        }
                        return cache.CheckAttrIsValidate(index, out attrs);
                }
                public static bool ValidateMethod(MethodInfo method, object[] values, out string error)
                {
                        long id = method.MethodHandle.Value.ToInt64();
                        if (!_ValidateCache.TryGetValue(id, out MethodValidateCache cache))
                        {
                                cache = _ValidateCache.GetOrAdd(id, new MethodValidateCache(method));
                                cache.InitValidate();
                        }
                        return cache.ValidateData(values, out error);
                }
        }
}
