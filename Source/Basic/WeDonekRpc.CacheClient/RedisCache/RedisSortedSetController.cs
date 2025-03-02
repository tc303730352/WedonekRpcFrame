using System;
using System.Collections.Generic;
using CSRedis;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.CacheClient.Redis;
using WeDonekRpc.Helper;

namespace WeDonekRpc.CacheClient.RedisCache
{
    internal class RedisSortedSetController : IRedisSortedSetController
    {
        private readonly CSRedisClient _Client;
        public RedisSortedSetController ()
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
        public long Union<T> (string destination, decimal[] weight, RedisAggregate aggregate, params string[] key)
        {
            key = RpcCacheService.FormatKey(key);
            destination = RpcCacheService.FormatKey(destination);
            return this._Client.ZUnionStore(destination, weight, aggregate, key);
        }
        public long Length (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.ZCard(key);
        }
        public long? Rank<T> (string key, T val)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.ZRank(key, val);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"> '(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
        /// <param name="max"> '(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
        /// <returns></returns>
        public long CountByLex (string key, string min, string max)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.ZLexCount(key, min, max);
        }
        public long RemoveByRank<T> (string key, long start, int end)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.ZRemRangeByRank(key, start, end);
        }
        public long Remove<T> (string key, params T[] vals)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.ZRem(key, vals);
        }
        public long RemoveByScore (string key, decimal min, decimal max)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.ZRemRangeByScore(key, min, max);
        }
        public long RemoveByLex (string key, string min, string max)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.ZRemRangeByLex(key, min, max);
        }
        public decimal? Score<T> (string key, T value)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.ZScore(key, value);
        }
        public RedisScan<(T member, decimal score)> Gets<T> (string key, long index, string pattern = null, long? count = null)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.ZScan<T>(key, index, pattern, count);
        }
        public string[] RankByScore (string key, decimal max, decimal min, long? count = null, long offset = 0)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.ZRevRangeByScore(key, max, min, count, offset);
        }
        public T[] RnankByScore<T> (string key, decimal max, decimal min, long? count = null, long offset = 0)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.ZRevRangeByScore<T>(key, max, min, count, offset);
        }
        public string[] GetRanks (string key, long start, long end)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.ZRevRange(key, start, end);
        }
        public T[] GetRanks<T> (string key, long start, long end)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.ZRevRange<T>(key, start, end);
        }
        public T[] Gets<T> (string key, long start, long end)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.ZRange<T>(key, start, end);
        }
        public string[] Gets (string key, long start, long end)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.ZRange(key, start, end);
        }
        public long Count (string key, decimal start, decimal end)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.ZCount(key, start, end);
        }
        public long Count (string key, string start, string end)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.ZCount(key, start, end);
        }
        public long Add (string key, object name, decimal val)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.ZAdd(key,
            [
                new(val, name)
            ]);
        }
        public long Add<T> (string key, Dictionary<T, decimal> dic)
        {
            key = RpcCacheService.FormatKey(key);
            (decimal, object)[] args = dic.ConvertAll<KeyValuePair<T, decimal>, (decimal, object)>(a =>
            {
                return new(a.Value, a.Key);
            });
            return this._Client.ZAdd(key, args);
        }
    }
}
