using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;

namespace WeDonekRpc.Helper.Reflection
{
    public class ReflectionHepler
    {
        private static readonly ConcurrentDictionary<string, IReflectionBody> _ClassCache = new ConcurrentDictionary<string, IReflectionBody>();
        internal static bool IsEquals (Type sourceType, Type otherType, object source, object other, bool? isExclude = null, params string[] pros)
        {
            if (sourceType == otherType && source.Equals(other))
            {
                return true;
            }
            else if (source == null)
            {
                return false;
            }
            IReflectionBody sou = _GetClassBody(sourceType);
            IReflectionBody ot = _GetClassBody(otherType);
            return sou.IsEquals(ot, source, other, isExclude, pros);
        }
        internal static bool IsEquals (Type sourceType, object source, object other, bool? isExclude = null, params string[] pros)
        {
            if (source.Equals(other))
            {
                return true;
            }
            IReflectionBody sou = _GetClassBody(sourceType);
            return sou.IsEquals(source, other, isExclude, pros);
        }
        public static IReflectionBody GetReflection (Type type)
        {
            return _GetClassBody(type);
        }
        public static IFastGetProperty GetFastGetPro (Type type, string name)
        {
            if (_ClassCache.TryGetValue(type.FullName, out IReflectionBody body))
            {
                return body.GetProperty(name);
            }
            PropertyInfo pro = type.GetProperty(name);
            return new FastGetProperty(pro);
        }
        private static IReflectionBody _GetClassBody (Type type)
        {
            if (_ClassCache.TryGetValue(type.FullName, out IReflectionBody body))
            {
                return body;
            }
            return _ClassCache.GetOrAdd(type.FullName, new ReflectionBody(type));
        }
        public static bool IsEquals<Source> (Source one, Source two, bool? isExclude, string[] pros)
            where Source : class
        {
            return IsEquals(typeof(Source), one, two, isExclude, pros);
        }
        public static string ToMd5<T> (T source, params string[] remove)
        {
            IReflectionBody body = _GetClassBody(typeof(T));
            string str = body.ToString(source, remove);
            return Tools.GetMD5(str);
        }
        public static string ToString<T> (T source, params string[] remove)
        {
            IReflectionBody body = _GetClassBody(typeof(T));
            return body.ToString(source, remove);
        }
        public static bool IsEquals<Source, Other> (Source source, Other other, bool? isExclude, params string[] pros)
            where Source : class
        {
            return IsEquals(typeof(Source), typeof(Other), source, other, isExclude, pros);
        }

        internal static void ToString (Type type, object val, StringBuilder str)
        {
            IReflectionBody body = _GetClassBody(type);
            body.ToString(val, str);
        }
        public static bool TryGetValue (Type type, object data, string name, out object val)
        {
            IReflectionBody body = _GetClassBody(type);
            if (body.TryGet(name, out IReflectionProperty property))
            {
                val = property.GetValue(data);
                return true;
            }
            val = null;
            return false;
        }
        public static object GetValue (Type type, object data, string name)
        {
            IReflectionBody body = _GetClassBody(type);
            return body.GetValue(data, name);
        }
    }
}
