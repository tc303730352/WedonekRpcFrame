using System;

namespace WeDonekRpc.Helper
{
    public static class EnumLinq
    {
        public static Nullable<T> ParseEnum<T>(this string str, bool ignoreCase = false) where T : struct, Enum
        {
            if (str.IsNull())
            {
                return null;
            }
            else if (EnumHelper.TryParse<T>(str, ignoreCase, out T val))
            {
                return val;
            }
            return null;
        }
        public static T ParseEnum<T>(this string str, T def, bool ignoreCase = false) where T : struct, Enum
        {
            if (str.IsNull())
            {
                return def;
            }
            else if (EnumHelper.TryParse<T>(str, ignoreCase, out T val))
            {
                return val;
            }
            else
            {
                return def;
            }
        }
        public static string ToString<T>(this T val) where T : struct, Enum
        {
            return EnumHelper.GetName<T>(val);
        }
        public static T ParseEnum<T>(this int val) where T : struct, Enum
        {
            return EnumHelper.Parse<T>(val);
        }
        public static string[] GetEnumName<T>(this int[] vals) where T : struct, Enum
        {
            Type type = typeof(T);
            return vals.ConvertAll(a => EnumHelper.GetName(type, a));
        }
        public static T ParseEnum<T>(this short val) where T : struct, Enum
        {
            return EnumHelper.Parse<T>(val);
        }
        public static T ParseEnum<T>(this byte val) where T : struct, Enum
        {
            return EnumHelper.Parse<T>(val);
        }
        public static T ParseEnum<T>(this long val) where T : struct, Enum
        {
            return EnumHelper.Parse<T>(val);
        }
    }
}
