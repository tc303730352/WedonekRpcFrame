using System;
using WeDonekRpc.Helper.Json;

namespace WeDonekRpc.Helper
{
    /// <summary>
    /// 操作类属性
    /// </summary>
    public class ModelHelper
    {
        public static T GetModel<T> (object res)
        {
            return res != null && DBNull.Value != res ? (T)_GetModel(typeof(T), res) : default;
        }
        public static object ChangeType (Type type, object res)
        {
            return res != null && DBNull.Value != res ? _GetModel(type, res) : null;
        }
        private static object _GetModel (Type type, object res)
        {
            if (PublicDataDic.UriType == type)
            {
                string uri = (string)res;
                return uri != string.Empty ? new Uri(uri) : null;
            }
            else if (PublicDataDic.GuidType == type)
            {
                string val = (string)res;
                return val != string.Empty ? new Guid(val) : Guid.Empty;
            }
            else if (type == PublicDataDic.StrType)
            {
                return res.ToString();
            }
            else if (type.IsClass)
            {
                string json = (string)res;
                return json != string.Empty ? JsonTools.Json(json, type) : null;
            }
            else if (type.IsEnum)
            {
                return EnumHelper.ToObject(type, res);
            }
            else if (type == PublicDataDic.DateTimeType)
            {
                return _FormatTime(res);
            }
            else
            {
                return Convert.ChangeType(res, type);
            }
        }

        private static DateTime _FormatTime (object res)
        {
            Type type = res.GetType();
            if (type == PublicDataDic.DateTimeType)
            {
                DateTime time = (DateTime)res;
                return time == Tools.SqlMinTime ? DateTime.MinValue : time;
            }
            else if (type == PublicDataDic.LongType)
            {
                return Tools.GetTimeStamp((long)res);
            }
            else if (type == PublicDataDic.StrType)
            {
                return DateTime.Parse((string)res);
            }
            return DateTime.MinValue;
        }

    }
}
