using System;
using CSRedis;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.CacheClient.Redis;

namespace WeDonekRpc.CacheClient.RedisCache
{
    internal class RedisSetController : IRedisSetController
    {
        private readonly CSRedisClient _Client;
        public RedisSetController ()
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
        public string[] Union (string[] key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.SUnion(key);
        }
        public long Union<T> (string destination, params string[] key)
        {
            key = RpcCacheService.FormatKey(key);
            destination = RpcCacheService.FormatKey(destination);
            return this._Client.SUnionStore(destination, key);
        }
        public T[] Union<T> (string[] key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.SUnion<T>(key);
        }
        public long Length (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.SCard(key);
        }
        public string GetRandom (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.SRandMember(key);
        }
        public T GetRandom<T> (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.SRandMember<T>(key);
        }
        public string[] GetRandoms (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.SRandMembers(key);
        }
        public T[] GetRandoms<T> (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.SRandMembers<T>(key);
        }
        public string[] Gets (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.SMembers(key);
        }
        public T[] Gets<T> (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.SMembers<T>(key);
        }
        public string[] Intersects (string[] keys)
        {
            keys = RpcCacheService.FormatKey(keys);
            return this._Client.SInter(keys);
        }
        public long Intersects (string destination, string[] keys)
        {
            keys = RpcCacheService.FormatKey(keys);
            destination = RpcCacheService.FormatKey(destination);
            return this._Client.SInterStore(destination, keys);
        }
        public long Remove<T> (string key, params T[] vals)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.SRem(key, vals);
        }
        public bool Move<T> (string key, string destination, object val)
        {
            key = RpcCacheService.FormatKey(key);
            destination = RpcCacheService.FormatKey(destination);
            return this._Client.SMove(key, destination, val);
        }
        public long Add<T> (string key, params T[] val)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.SAdd(key, val);
        }
        public bool Exists<T> (string key, T value)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.SIsMember(key, value);
        }
        public long Differenced (string destination, params string[] keys)
        {
            keys = RpcCacheService.FormatKey(keys);
            destination = RpcCacheService.FormatKey(destination);
            return this._Client.SDiffStore(destination, keys);
        }
        public string[] Differenced (params string[] keys)
        {
            keys = RpcCacheService.FormatKey(keys);
            return this._Client.SDiff(keys);
        }
        public T[] Differenced<T> (params string[] keys)
        {
            keys = RpcCacheService.FormatKey(keys);
            return this._Client.SDiff<T>(keys);
        }
        public RedisScan<T> Gets<T> (string key, long index, string pattern = null, long? count = null)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.SScan<T>(key, index, pattern, count);
        }
        public string Pop (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.SPop(key);
        }
        public string[] Pop (string key, long count)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.SPop(key, count);
        }
        public T Pop<T> (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.SPop<T>(key);
        }
        public T[] Pop<T> (string key, long count)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.SPop<T>(key, count);
        }
    }
}
