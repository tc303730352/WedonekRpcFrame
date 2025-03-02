using System;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Json;

namespace WeDonekRpc.CacheClient.Redis
{
    internal class RedisTools
    {
        public static string Serializable<T> (T data)
        {
            Type type = typeof(T);
            if (type.FullName == PublicDataDic.GuidType.FullName)
            {
                return data.ToString();
            }
            else if (type.FullName == PublicDataDic.UriType.FullName)
            {
                return data.ToString();
            }
            else if (type.FullName == PublicDataDic.DateTimeType.FullName)
            {
                return data.ToString();
            }
            else if (type.IsClass && type.Name != PublicDataDic.StringTypeName)
            {
                return JsonTools.Json(data);
            }
            else
            {
                return data as string;
            }
        }
    }
}
