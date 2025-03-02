using System;

using WeDonekRpc.Helper;
namespace WeDonekRpc.Modular.Helper
{
    internal class HelperTools
    {
        public static bool CheckIsJson (Type type)
        {
            if (type.IsPrimitive)
            {
                return false;
            }
            else if (type.IsEnum || type == PublicDataDic.StrType || type == PublicDataDic.GuidType || type == PublicDataDic.UriType || type == PublicDataDic.DateTimeType)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        internal static T GetValue<T> (string value)
        {
            Type type = typeof(T);
            return (T)GetValue(type, value);
        }
        internal static object GetValue (Type type, string value)
        {
            if (type.IsEnum)
            {
                return Enum.Parse(type, value);
            }
            else if (type == PublicDataDic.StrType)
            {
                return value;
            }
            else if (type == PublicDataDic.LongType)
            {
                return long.Parse(value);
            }
            else if (type == PublicDataDic.IntType)
            {
                return int.Parse(value);
            }
            else if (type == PublicDataDic.ShortType)
            {
                return short.Parse(value);
            }
            else if (type == PublicDataDic.BoolType)
            {
                return bool.Parse(value);
            }
            else if (type == PublicDataDic.UriType)
            {
                return new Uri(value);
            }
            else if (type == PublicDataDic.DateTimeType)
            {
                return DateTime.Parse(value);
            }
            else if (type.Name == PublicDataDic.DecimalTypeName)
            {
                return decimal.Parse(value);
            }
            else if (type.Name == PublicDataDic.ByteTypeName)
            {
                return byte.Parse(value);
            }
            else if (type == PublicDataDic.GuidType)
            {
                return Guid.Parse(value);
            }
            return Convert.ChangeType(value, type);
        }
    }
}
