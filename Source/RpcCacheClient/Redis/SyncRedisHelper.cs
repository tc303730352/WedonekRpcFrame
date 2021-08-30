using System;
using System.Linq;

using StackExchange.Redis;

namespace RpcCacheClient.Redis
{
        internal class SyncRedisHelper : RedisHelper
        {
                public static bool IsExitKey(IDatabase client, string key, string colName)
                {
                        return client.HashExists(key, colName);
                }
                public static bool IsExitKey(IDatabase client, string key)
                {
                        return client.KeyExists(key);
                }
                public static bool SetExpire(IDatabase client, string key, DateTime time)
                {
                        return client.KeyExpire(key, time);
                }

                #region Strings
                public static double Increment(IDatabase client, string key, double num)
                {
                        return client.StringIncrement(key, num);
                }
                public static long Increment(IDatabase client, string key, long num)
                {
                        return client.StringIncrement(key, num);
                }
                public static double Decrement(IDatabase client, string key, double num)
                {
                        return client.StringDecrement(key, num);
                }
                public static long Decrement(IDatabase client, string key, long num)
                {
                        return client.StringDecrement(key, num);
                }
                public static bool TryGet<T>(IDatabase client, string key, out T data)
                {
                        RedisValue value = client.StringGet(key);
                        return RedisTools.GetT(value, out data);
                }
                public static bool Set<T>(IDatabase client, string key, T data)
                {
                        return client.StringSet(key, RedisTools.Serializable<T>(data));
                }
                public static bool Set<T>(IDatabase client, string key, T data, TimeSpan expiresAt)
                {
                        return client.StringSet(key, RedisTools.Serializable<T>(data), expiresAt);
                }
                public static bool Remove(IDatabase client, string key)
                {
                        return client.KeyDelete(key);
                }
                public static long Remove(IDatabase client, string[] key)
                {
                        return client.KeyDelete(key.Cast<RedisKey>().ToArray());
                }
                public static bool Add<T>(IDatabase client, string key, T data)
                {
                        return client.StringSet(key, RedisTools.Serializable<T>(data), null, When.NotExists);
                }
                public static bool Replace<T>(IDatabase client, string key, T data, TimeSpan expiresAt)
                {
                        return client.StringSet(key, RedisTools.Serializable<T>(data), expiresAt, When.Exists);
                }

                public static bool Replace<T>(IDatabase client, string key, T data)
                {
                        return client.StringSet(key, RedisTools.Serializable<T>(data), null, When.Exists);
                }
                public static bool Add<T>(IDatabase client, string key, T data, TimeSpan expiresAt)
                {
                        return client.StringSet(key, RedisTools.Serializable<T>(data), expiresAt, When.NotExists);
                }
                #endregion
                public static string[] FindKey(IDatabase client, string pattern)
                {
                        RedisResult result = client.ScriptEvaluate(LuaScript.Prepare("local res=redis.call('KEYS', @keypattern) return res"), new { @keypattern = pattern });
                        return result.IsNull ? (new string[0]) : (string[])result;
                }
                #region Hash

                public static double Increment(IDatabase client, string key, string colname, double num)
                {
                        return client.HashIncrement(key, colname, num);
                }
                public static long Increment(IDatabase client, string key, string colname, long num)
                {
                        return client.HashIncrement(key, colname, num);
                }
                public static double Decrement(IDatabase client, string key, string colname, double num)
                {
                        return client.HashDecrement(key, colname, num);
                }
                public static long Decrement(IDatabase client, string key, string colname, long num)
                {
                        return client.HashDecrement(key, colname, num);
                }
                public static bool Add<T>(IDatabase client, string key, string colname, T data)
                {
                        return client.HashSet(key, colname, RedisTools.Serializable<T>(data), When.NotExists);
                }
                public static bool Set<T>(IDatabase client, string key, string colname, T data)
                {
                        return client.HashSet(key, colname, RedisTools.Serializable<T>(data));
                }
                public static bool Replace<T>(IDatabase client, string key, string colname, T data)
                {
                        return client.HashSet(key, colname, RedisTools.Serializable<T>(data), When.Exists);
                }
                public static bool Remove(IDatabase client, string key, string colname)
                {
                        return client.HashDelete(key, colname);
                }

                public static bool TryGet<T>(IDatabase client, string key, string colname, out T data)
                {
                        RedisValue value = client.HashGet(key, colname);
                        return RedisTools.GetT(value, out data);
                }
                public static bool TryGet<T>(IDatabase client, string key, out T[] data)
                {
                        RedisValue[] value = client.HashValues(key);
                        return RedisTools.GetT<T>(value, out data);
                }
                public static bool TryGetColName(IDatabase client, string key, out string[] colname)
                {
                        RedisValue[] value = client.HashKeys(key);
                        return RedisTools.GetT<string>(value, out colname);
                }
                public static long GetCount(IDatabase client, string key)
                {
                        return client.HashLength(key);
                }
                public static long Remove(IDatabase client, string key, string[] colname)
                {
                        return client.HashDelete(key, colname.Cast<RedisValue>().ToArray());
                }
                #endregion

                #region SyncList
                public static bool GetList<T>(IDatabase client, string key, int index, out T data)
                {
                        RedisValue value = client.ListGetByIndex(key, index);
                        return RedisTools.GetT<T>(value, out data);
                }
                public static bool GetList<T>(IDatabase client, string key, out T[] data)
                {
                        RedisValue[] value = client.ListRange(key);
                        return RedisTools.GetT<T>(value, out data);
                }
                public static bool GetList<T>(IDatabase client, string key, int index, int size, out T[] data)
                {
                        RedisValue[] value = client.ListRange(key, (index - 1) * size, index * size);
                        return RedisTools.GetT<T>(value, out data);
                }
                public static void SetList<T>(IDatabase client, string key, int index, T data)
                {
                        client.ListSetByIndex(key, index, RedisTools.Serializable<T>(data));
                }
                public static long ListAddLeft<T>(IDatabase client, string key, T data)
                {
                        return client.ListLeftPush(key, RedisTools.Serializable<T>(data));
                }
                public static long ListAddRight<T>(IDatabase client, string key, T data)
                {
                        return client.ListRightPush(key, RedisTools.Serializable<T>(data));
                }
                public static long ListAddLeft<T>(IDatabase client, string key, T[] data)
                {
                        return client.ListLeftPush(key, Array.ConvertAll<T, RedisValue>(data, a => RedisTools.Serializable<T>(a)));
                }
                public static bool ListLeftPop<T>(IDatabase client, string key, out T data)
                {
                        RedisValue value = client.ListLeftPop(key);
                        return RedisTools.GetT<T>(value, out data);
                }
                public static bool ListRightPop<T>(IDatabase client, string key, out T data)
                {
                        RedisValue value = client.ListRightPop(key);
                        return RedisTools.GetT<T>(value, out data);
                }
                public static long ListAddRight<T>(IDatabase client, string key, T[] data)
                {
                        return client.ListRightPush(key, Array.ConvertAll<T, RedisValue>(data, a => RedisTools.Serializable<T>(a)));
                }
                public static long GetListCount(IDatabase client, string key)
                {
                        return client.ListLength(key);
                }
                public static void ListTop(IDatabase client, string key, int top)
                {
                        client.ListTrim(key, 0, top);
                }
                public static long ListRemove<T>(IDatabase client, string key, T data)
                {
                        return client.ListRemove(key, RedisTools.Serializable<T>(data));
                }
                public static void ListTop(IDatabase client, string key, int start, int end)
                {
                        client.ListTrim(key, start, end);
                }
                #endregion

        }
}
