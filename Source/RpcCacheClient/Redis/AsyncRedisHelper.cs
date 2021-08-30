using System;
using System.Linq;
using System.Threading.Tasks;

using StackExchange.Redis;

namespace RpcCacheClient.Redis
{
        internal class AsyncRedisHelper : RedisHelper
        {

                public static async Task<bool> IsExitKey(IDatabaseAsync client, string key, string colName)
                {
                        return await client.HashExistsAsync(key, colName);
                }
                public static async Task<bool> IsExitKey(IDatabaseAsync client, string key)
                {
                        return await client.KeyExistsAsync(key);
                }
                #region AsyncList
                public static async Task<T> GetList<T>(IDatabaseAsync client, string key, int index)
                {
                        RedisValue value = await client.ListGetByIndexAsync(key, index);
                        return RedisTools.GetT<T>(value);
                }
                public static async Task<T[]> GetList<T>(IDatabaseAsync client, string key)
                {
                        RedisValue[] value = await client.ListRangeAsync(key);
                        return RedisTools.GetT<T>(value, out T[] data) ? data : null;
                }
                public static async Task<T[]> GetList<T>(IDatabaseAsync client, string key, int index, int size)
                {
                        RedisValue[] value = await client.ListRangeAsync(key, (index - 1) * size, index * size);
                        return RedisTools.GetT(value, out T[] data) ? data : null;
                }
                public static async void SetList<T>(IDatabaseAsync client, string key, int index, T data)
                {
                        await client.ListSetByIndexAsync(key, index, RedisTools.Serializable<T>(data));
                }
                public static async Task<long> ListAddLeft<T>(IDatabaseAsync client, string key, T data)
                {
                        return await client.ListLeftPushAsync(key, RedisTools.Serializable<T>(data));
                }
                public static async Task<long> ListAddRight<T>(IDatabaseAsync client, string key, T data)
                {
                        return await client.ListRightPushAsync(key, RedisTools.Serializable<T>(data));
                }
                public static async Task<long> ListAddLeft<T>(IDatabaseAsync client, string key, T[] data)
                {
                        return await client.ListLeftPushAsync(key, Array.ConvertAll<T, RedisValue>(data, a => RedisTools.Serializable<T>(a)));
                }
                public static async Task<T> ListLeftPop<T>(IDatabaseAsync client, string key)
                {
                        RedisValue value = await client.ListLeftPopAsync(key);
                        return RedisTools.GetT<T>(value);
                }
                public static async Task<T> ListRightPop<T>(IDatabaseAsync client, string key)
                {
                        RedisValue value = await client.ListRightPopAsync(key);
                        return RedisTools.GetT<T>(value);
                }
                public static async Task<long> ListAddRight<T>(IDatabaseAsync client, string key, T[] data)
                {
                        return await client.ListRightPushAsync(key, Array.ConvertAll<T, RedisValue>(data, a => RedisTools.Serializable<T>(a)));
                }
                public static async Task<long> GetListCount(IDatabaseAsync client, string key)
                {
                        return await client.ListLengthAsync(key);
                }
                public static async void ListTop(IDatabaseAsync client, string key, int top)
                {
                        await client.ListTrimAsync(key, 0, top);
                }
                public static async void ListTop(IDatabaseAsync client, string key, int start, int end)
                {
                        await client.ListTrimAsync(key, start, end);
                }
                public static async Task<long> ListRemove<T>(IDatabaseAsync client, string key, T data)
                {
                        return await client.ListRemoveAsync(key, RedisTools.Serializable<T>(data));
                }
                #endregion
                public static async Task<string[]> FindKey(IDatabaseAsync client, string pattern)
                {
                        RedisResult result = await client.ScriptEvaluateAsync(LuaScript.Prepare("local res=redis.call('KEYS', @keypattern) return res"), new { @keypattern = pattern });
                        return result.IsNull ? (new string[0]) : (string[])result;
                }

                #region AsyncHash
                public static async Task<double> Increment(IDatabaseAsync client, string key, string colname, double num)
                {
                        return await client.HashIncrementAsync(key, colname, num);
                }
                public static async Task<long> Increment(IDatabaseAsync client, string key, string colname, long num)
                {
                        return await client.HashIncrementAsync(key, colname, num);
                }
                public static async Task<double> Decrement(IDatabaseAsync client, string key, string colname, double num)
                {
                        return await client.HashDecrementAsync(key, colname, num);
                }
                public static async Task<long> Decrement(IDatabaseAsync client, string key, string colname, long num)
                {
                        return await client.HashDecrementAsync(key, colname, num);
                }
                public static async Task<bool> Add<T>(IDatabaseAsync client, string key, string colname, T data)
                {
                        return await client.HashSetAsync(key, colname, RedisTools.Serializable<T>(data), When.NotExists);
                }
                public static async Task<bool> Set<T>(IDatabaseAsync client, string key, string colname, T data)
                {
                        return await client.HashSetAsync(key, colname, RedisTools.Serializable<T>(data));
                }
                public static async Task<bool> Replace<T>(IDatabaseAsync client, string key, string colname, T data)
                {
                        return await client.HashSetAsync(key, colname, RedisTools.Serializable<T>(data), When.Exists);
                }
                public static async Task<bool> Remove(IDatabaseAsync client, string key, string colname)
                {
                        return await client.HashDeleteAsync(key, colname);
                }

                public static async Task<T> Get<T>(IDatabaseAsync client, string key, string colname)
                {
                        RedisValue value = await client.HashGetAsync(key, colname);
                        return RedisTools.GetT(value, out T data) ? data : default;
                }
                public static async Task<T[]> GetAll<T>(IDatabaseAsync client, string key)
                {
                        RedisValue[] value = await client.HashValuesAsync(key);
                        return RedisTools.GetT(value, out T[] data) ? data : null;
                }
                public static async Task<string[]> GetColName(IDatabaseAsync client, string key)
                {
                        RedisValue[] value = await client.HashKeysAsync(key);
                        if (RedisTools.GetT<string>(value, out string[] colname))
                        {
                                return colname;
                        }
                        return null;
                }
                public static async Task<long> GetCount(IDatabaseAsync client, string key)
                {
                        return await client.HashLengthAsync(key);
                }
                public static async Task<long> Remove(IDatabaseAsync client, string key, string[] colname)
                {
                        return await client.HashDeleteAsync(key, colname.Cast<RedisValue>().ToArray());
                }
                #endregion

                #region AsyncString
                public static async Task<double> Increment(IDatabaseAsync client, string key, double num)
                {
                        return await client.StringIncrementAsync(key, num);
                }
                public static async Task<long> Increment(IDatabaseAsync client, string key, long num)
                {
                        return await client.StringIncrementAsync(key, num);
                }
                public static async Task<double> Decrement(IDatabaseAsync client, string key, double num)
                {
                        return await client.StringDecrementAsync(key, num);
                }
                public static async Task<long> Decrement(IDatabaseAsync client, string key, long num)
                {
                        return await client.StringDecrementAsync(key, num);
                }
                public static async Task<T> Get<T>(IDatabaseAsync client, string key)
                {
                        RedisValue res = await client.StringGetAsync(key);
                        return RedisTools.GetT<T>(res);
                }
                public static async Task<bool> Set<T>(IDatabaseAsync client, string key, T data)
                {
                        return await client.StringSetAsync(key, RedisTools.Serializable<T>(data));
                }
                public static async Task<bool> Set<T>(IDatabaseAsync client, string key, T data, TimeSpan expiresAt)
                {
                        return await client.StringSetAsync(key, RedisTools.Serializable<T>(data), expiresAt);
                }
                public static async Task<bool> Remove(IDatabaseAsync client, string key)
                {
                        return await client.KeyDeleteAsync(key);
                }
                public static async Task<long> Remove(IDatabaseAsync client, string[] key)
                {
                        return await client.KeyDeleteAsync(key.Cast<RedisKey>().ToArray());
                }
                public static async Task<bool> Add<T>(IDatabaseAsync client, string key, T data)
                {
                        return await client.StringSetAsync(key, RedisTools.Serializable<T>(data), null, When.NotExists);
                }
                public static async Task<bool> Replace<T>(IDatabaseAsync client, string key, T data, TimeSpan expiresAt)
                {
                        return await client.StringSetAsync(key, RedisTools.Serializable<T>(data), expiresAt, When.Exists);
                }

                public static async Task<bool> Replace<T>(IDatabaseAsync client, string key, T data)
                {
                        return await client.StringSetAsync(key, RedisTools.Serializable<T>(data), null, When.Exists);
                }
                public static async Task<bool> Add<T>(IDatabaseAsync client, string key, T data, TimeSpan expiresAt)
                {
                        return await client.StringSetAsync(key, RedisTools.Serializable<T>(data), expiresAt, When.NotExists);
                }
                #endregion
        }
}
