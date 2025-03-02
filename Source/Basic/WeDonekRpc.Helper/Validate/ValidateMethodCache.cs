using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace WeDonekRpc.Helper.Validate
{
    internal class ValidateMethodCache
    {
        private static readonly ConcurrentDictionary<string, EntrustMethod> _Method = new ConcurrentDictionary<string, EntrustMethod>();

        private static MethodInfo _FindMethod (Type type, string func)
        {
            MethodInfo fun = type.GetMethod(func, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod);
            if (fun == null)
            {
                if (type.BaseType != null && type.BaseType.Name != "System.Object")
                {
                    return _FindMethod(type.BaseType, func);
                }
                return null;
            }
            return fun;
        }
        public static bool GetMethod (object source, Type attrType, string func, out EntrustMethod method)
        {
            Type type = source.GetType();
            string name = string.Join("_", type.FullName, func, attrType.Name);
            if (_Method.TryGetValue(name, out method))
            {
                return true;
            }
            MethodInfo fun = _FindMethod(type, func);
            if (fun == null)
            {
                return false;
            }
            method = new EntrustMethod(fun, type, attrType);
            _ = _Method.TryAdd(name, method);
            return true;
        }
    }
}
