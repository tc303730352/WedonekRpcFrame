using System;
using System.Collections.Generic;
using EnumsNET;
namespace WeDonekRpc.Helper
{
    public class EnumHelper
    {
        public static T ToObject<T>(object obj)
        {
            return Enums.ToObjectUnsafe<T>(obj);
        }
        public static T Parse<T>(string val) where T : struct, Enum
        {
            return Enums.Parse<T>(val);
        }
        public static T Parse<T>(string val, bool ignoreCase) where T : struct, Enum
        {
            return Enums.Parse<T>(val, ignoreCase);
        }
        public static IReadOnlyList<T> GetValues<T>() where T : struct, Enum
        {
            return Enums.GetValues<T>();
        }
        public static int[] GetValues(Type type)
        {
            IReadOnlyList<object> vals = Enums.GetValues(type);
            int[] t = new int[vals.Count];
            int k = 0;
            foreach (object i in vals)
            {
                t[k++] = (int)i;
            }
            return t;
        }

        public static string GetName<T>(T val) where T : struct, Enum
        {
            return Enums.GetName<T>(val);
        }
        public static string GetName(Type type, int val)
        {
            return Enums.GetName(type, val);
        }
        public static object GetFistValue(Type type)
        {
            return Enums.GetValues(type)[0];
        }
        public static T Parse<T>(int val)
        {
            return Enums.ToObjectUnsafe<T>(val);
        }
        public static object Parse(int val, Type type)
        {
            return Enums.ToObject(type, val);
        }
        public static T Parse<T>(long val)
        {
            return Enums.ToObjectUnsafe<T>(val);
        }
        public static T Parse<T>(short val)
        {
            return Enums.ToObjectUnsafe<T>(val);
        }
        public static T Parse<T>(byte val)
        {
            return Enums.ToObjectUnsafe<T>(val);
        }
        public static object ToObject(Type type, object obj)
        {
            return Enums.ToObject(type, obj);
        }

        public static object Parse(Type type, string str)
        {
            return Enums.Parse(type, str);
        }
        public static int ToInt(Type type, object val)
        {
            return Enums.ToInt32(type, val);
        }
        public static bool TryParse(Type type, string str, out object val)
        {
            return Enums.TryParse(type, str, out val);
        }
        public static bool TryParse<T>(string str, bool ignoreCase, out T val) where T : struct, Enum
        {
            return Enums.TryParse<T>(str, ignoreCase, out val);
        }

        public static object ToNumber(Type enumType, object value)
        {
            return Enums.GetUnderlyingValue(enumType, value);
        }
    }
}
