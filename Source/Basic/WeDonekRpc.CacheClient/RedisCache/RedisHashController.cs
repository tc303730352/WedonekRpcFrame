using System;
using System.Collections.Generic;
using CSRedis;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.CacheClient.Redis;

namespace WeDonekRpc.CacheClient.RedisCache
{
    internal class RedisHashController : IRedisHashController
    {
        private readonly CSRedisClient _Client;
        public RedisHashController ()
        {
            this._Client = RedisCommon.RedisClient;
        }
        public bool Exists (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.Exists(key);
        }
        public bool Exists (string key, string fieId)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.HExists(key, fieId);
        }
        public bool SetExpire (string key, TimeSpan time)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.Expire(key, time);
        }

        public bool Set (string key, string field, object value)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.HSet(key, field, value);
        }
        public bool Set (string key, Dictionary<string, object> values)
        {
            key = RpcCacheService.FormatKey(key);
            object[] objs = new object[values.Count * 2];
            int index = 0;
            foreach (KeyValuePair<string, object> i in values)
            {
                objs[index++] = i.Key;
                objs[index++] = i.Value;
            }
            return this._Client.HMSet(key, objs);
        }
        public T Get<T> (string key, string field)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.HGet<T>(key, field);
        }
        public T[] Gets<T> (string key, params string[] field)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.HMGet<T>(key, field);
        }
        public string Get (string key, string field)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.HGet(key, field);
        }
        public string[] Gets (string key, params string[] field)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.HMGet(key, field);
        }
        public Dictionary<string, T> Get<T> (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.HGetAll<T>(key);
        }
        public Dictionary<string, string> Get (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.HGetAll(key);
        }
        public long Increment (string key, string field)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.HIncrBy(key, field, 1);
        }
        public long Decrement (string key, string field)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.HIncrBy(key, field, -1);
        }
        public decimal IncrBy (string key, string field, decimal num)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.HIncrByFloat(key, field, num);
        }
        public long IncrBy (string key, string field, long num)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.HIncrBy(key, field, num);
        }
        public long Length (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.HLen(key);
        }
        public long Remove (string key, params string[] fieId)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.HDel(key, fieId);
        }
        public bool Remove (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.Del(key) == 1;
        }
        public string[] GetValues (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.HVals(key);
        }
        public T[] GetValues<T> (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.HVals<T>(key);
        }
        public string[] GetKeys (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.HKeys(key);
        }
        public bool Add (string key, string field, object value)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.HSetNx(key, field, value);
        }
    }
}
