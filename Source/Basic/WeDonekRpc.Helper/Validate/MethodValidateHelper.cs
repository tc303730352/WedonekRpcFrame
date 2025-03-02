using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Helper.Validate
{
    public class MethodValidateHelper
    {
        private static readonly ConcurrentDictionary<long, MethodValidateCache> _ValidateCache = new ConcurrentDictionary<long, MethodValidateCache>();

        internal static Dictionary<int, IValidateData> GetValidateList ( MethodInfo method )
        {
            Dictionary<int, IValidateData> attrs = [];
            ParameterInfo[] param = method.GetParameters().FindAll(a => !a.IsOut && !a.ParameterType.Name.EndsWith("&"));
            if ( param.Length == 0 )
            {
                return attrs;
            }
            param.ForEach(a =>
            {
                Type type = a.ParameterType;
                if ( type.IsArray )
                {
                    type = type.GetElementType();
                }
                else if ( type.IsGenericType && type.Name == "List`1" )
                {
                    type = type.GenericTypeArguments[0];
                }
                IValidateAttr[] attr = a.GetCustomAttributes<ValidateAttr>(false).ToArray();
                if ( DataValidateHepler.CheckIsValidate(type) )
                {
                    attrs.Add(a.Position, new ValidateAgent(a, attr));
                }
                else if ( attr.Length > 0 )
                {
                    attrs.Add(a.Position, new ValidateParameter(a, attr));
                }
            });
            return attrs;
        }
        public static bool CheckIsValidate ( MethodInfo method )
        {
            long id = method.MethodHandle.Value.ToInt64();
            if ( !_ValidateCache.TryGetValue(id, out MethodValidateCache cache) )
            {
                cache = _ValidateCache.GetOrAdd(id, new MethodValidateCache(method));
                cache.InitValidate();
            }
            return cache.AttrNum != 0;
        }
        public static bool CheckIsValidate ( MethodInfo method, int index, out IValidateAttr[] attrs )
        {
            long id = method.MethodHandle.Value.ToInt64();
            if ( !_ValidateCache.TryGetValue(id, out MethodValidateCache cache) )
            {
                cache = _ValidateCache.GetOrAdd(id, new MethodValidateCache(method));
                cache.InitValidate();
            }
            return cache.CheckAttrIsValidate(index, out attrs);
        }
        public static bool ValidateMethod ( MethodInfo method, object[] values, out string error )
        {
            long id = method.MethodHandle.Value.ToInt64();
            if ( !_ValidateCache.TryGetValue(id, out MethodValidateCache cache) )
            {
                cache = _ValidateCache.GetOrAdd(id, new MethodValidateCache(method));
                cache.InitValidate();
            }
            return cache.ValidateData(values, out error);
        }
    }
}
