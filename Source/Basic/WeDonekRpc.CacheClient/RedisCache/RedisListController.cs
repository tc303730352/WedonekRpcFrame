using System;
using CSRedis;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.CacheClient.Redis;

namespace WeDonekRpc.CacheClient.RedisCache
{
    internal class RedisListController : IRedisListController
    {
        protected readonly CSRedisClient _Client;
        public RedisListController ()
        {
            this._Client = RedisCommon.RedisClient;
        }
        public bool Exists (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.Exists(key);
        }
        public bool SetExpire (string key, TimeSpan time)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.Expire(key, time);
        }
        public T LeftPop<T> (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.LPop<T>(key);
        }
        public T RightPop<T> (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.RPop<T>(key);
        }
        public long Insert<T> (string key, params T[] value)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.LPush(key, value);
        }
        public long InsertExist<T> (string key, T value)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.LPushX(key, value);
        }
        public long Append<T> (string key, params T[] value)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.RPush(key, value);
        }
        public long AppendExist<T> (string key, T value)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.RPushX(key, value);
        }
        public long InsertAfter<T> (string key, int index, T value)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.LInsertAfter(key, index, value);
        }
        public long InsertBefore<T> (string key, int index, T value)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.LInsertBefore(key, index, value);
        }
        public T Get<T> (string key, int index)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.LIndex<T>(key, index);
        }
        public long Count (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.LLen(key);
        }
        public T[] Gets<T> (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.LRange<T>(key, 0, -1);
        }
        public T[] Gets<T> (string key, int start)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.LRange<T>(key, start, -1);
        }
        public T[] Gets<T> (string key, int start, int end)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.LRange<T>(key, start, end);
        }
        public bool Remove (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.Del(key) != 0;
        }
        public long Remove<T> (string key, T data, int count = 0)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.LRem(key, count, data);
        }
        public bool Trim (string key, int start, int end)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.LTrim(key, start, end);
        }
        public T LeftPopWait<T> (string key, int timeOut)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.BLPop<T>(timeOut, key);
        }
        public (string key, T value)? LeftPopWait<T> (int timeOut, params string[] keys)
        {
            keys = RpcCacheService.FormatKey(keys);
            return this._Client.BLPopWithKey<T>(timeOut, keys);
        }
        public T RightPopWait<T> (string key, int timeOut)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.BRPop<T>(timeOut, key);
        }
        public (string key, T value)? RightPopWait<T> (int timeOut, params string[] keys)
        {
            keys = RpcCacheService.FormatKey(keys);
            return this._Client.BRPopWithKey<T>(timeOut, keys);
        }
        public T RightPopWait<T> (string key, string dest, int timeOut)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.BRPopLPush<T>(key, dest, timeOut);
        }
        public T LeftPopWait<T> (string key, string dest, int timeOut)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.BRPopLPush<T>(key, dest, timeOut);
        }
    }
}
