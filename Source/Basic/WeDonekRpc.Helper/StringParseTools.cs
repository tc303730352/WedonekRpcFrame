using System;
using System.Collections.Generic;
using System.Reflection;
using WeDonekRpc.Helper.StringParse;

namespace WeDonekRpc.Helper
{
    public class StringParseTools
    {
        private static readonly Dictionary<Type, IStringParse> _dic = [];

        static StringParseTools ()
        {
            Assembly assembly = typeof(StringParseTools).Assembly;
            string name = "WeDonekRpc.Helper.StringParse.IStringParse";
            Type[] types = assembly.GetTypes().FindAll(a => a.Namespace == "WeDonekRpc.Helper.StringParse" && a.GetInterface(name) != null);
            types.ForEach(a =>
            {
                IStringParse obj = (IStringParse)Activator.CreateInstance(a);
                _dic.Add(obj.Type, obj);
            });

        }
        private static IStringParse _GetParse (Type type)
        {
            if (type.IsEnum)
            {
                return _dic[PublicDataDic.EnumType];
            }
            else if (_dic.TryGetValue(type, out IStringParse parse))
            {
                return parse;
            }
            else
            {
                return _dic[PublicDataDic.ObjectType];
            }
        }
        private static Type _FormatType (Type type)
        {
            if (type.IsGenericType && type.Name == "Nullable`1")
            {
                return type.GenericTypeArguments[0];
            }
            return type;
        }
        public static dynamic Parse (string str, Type type)
        {
            if (str == null)
            {
                throw new ErrorException("public.parse.string.null");
            }
            else if (type == PublicDataDic.StrType)
            {
                return str;
            }
            else if (type == PublicDataDic.CharType)
            {
                return str[0];
            }
            type = _FormatType(type);
            IStringParse parse = _GetParse(type);
            return parse.Parse(str, type);
        }
        public static bool TryParse (string str, Type type, out dynamic result)
        {
            if (str == null)
            {
                result = null;
                return false;
            }
            else if (type == PublicDataDic.StrType)
            {
                result = str;
                return true;
            }
            type = _FormatType(type);
            IStringParse parse = _GetParse(type);
            return parse.TryParse(str, type, out result);
        }
    }
}
