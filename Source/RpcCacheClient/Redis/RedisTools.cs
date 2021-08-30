using System;

using StackExchange.Redis;

using RpcHelper;

namespace RpcCacheClient.Redis
{
        internal class RedisTools
        {
                public static bool GetT<T>(RedisValue value, out T data)
                {
                        if (!value.HasValue || value.IsNull)
                        {
                                data = default;
                                return false;
                        }
                        else
                        {
                                data = RedisTools.GetT<T>(value);
                                return true;
                        }
                }
                public static bool GetT<T>(RedisValue[] value, out T[] data)
                {
                        if (value == null || value.Length == 0)
                        {
                                data = null;
                                return false;
                        }
                        data = value.ConvertAll(a =>
                        {
                                return RedisTools.GetT<T>(a);
                        });
                        return true;
                }
                public static T GetT<T>(RedisValue value)
                {
                        if (value.IsInteger)
                        {
                                return (T)Convert.ChangeType(value.Box(), typeof(T));
                        }
                        else
                        {
                                Type type = typeof(T);
                                if (type.FullName == PublicDataDic.GuidType.FullName)
                                {
                                        object obj = new Guid(value.ToString());
                                        return (T)obj;
                                }
                                else if (type.FullName == PublicDataDic.UriType.FullName)
                                {
                                        object obj = new Uri(value.ToString());
                                        return (T)obj;
                                }
                                else if (type.FullName == PublicDataDic.DateTimeType.FullName)
                                {
                                        object obj = DateTime.Parse(value.ToString());
                                        return (T)obj;
                                }
                                else if (type.IsClass)
                                {
                                        if (type.Name == PublicDataDic.StringTypeName)
                                        {
                                                object obj = value.ToString();
                                                return (T)obj;
                                        }
                                        else
                                        {
                                                return Tools.AutoDecompression<T>(value);
                                        }
                                }
                                else if (type.IsEnum)
                                {
                                        int val = (int)Convert.ChangeType(value, typeof(int));
                                        return (T)Enum.ToObject(type, val);
                                }
                                else
                                {
                                        return (T)Convert.ChangeType(value, typeof(T));
                                }
                        }
                }
                public static RedisValue Serializable<T>(T data)
                {
                        Type type = typeof(T);
                        if (type.FullName == PublicDataDic.GuidType.FullName)
                        {
                                return RedisValue.Unbox(data.ToString());
                        }
                        else if (type.FullName == PublicDataDic.UriType.FullName)
                        {
                                return RedisValue.Unbox(data.ToString());
                        }
                        else if (type.FullName == PublicDataDic.DateTimeType.FullName)
                        {
                                return RedisValue.Unbox(data.ToString());
                        }
                        return type.IsClass && type.Name != PublicDataDic.StringTypeName ? Tools.AutoCompression<T>(data) : RedisValue.Unbox(data);
                }
        }
}
