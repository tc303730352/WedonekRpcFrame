using System;
using System.Threading.Tasks;
using WeDonekRpc.CacheClient.Interface;

namespace WeDonekRpc.CacheClient.Redis
{
    internal class AsyncRedisCache : IAsyncCacheController
    {
        public CacheType CacheType => CacheType.Redis;

        public Task<bool> Add<T> (string key, T data, TimeSpan? expires = null)
        {
            return RedisCommon.AddAsync<T>(key, data, expires);
        }

        public Task<T> AddOrUpdate<T> (string key, T data, Func<T, T, T> upFunc, TimeSpan? expires = null)
        {
            return Task.Run<T>(() =>
            {
                if (RedisCommon.TryGet<T>(key, out T res))
                {
                    data = upFunc(res, data);
                    if (data == null || data.Equals(res))
                    {
                        return Task.FromResult(res);
                    }
                }
                return RedisCommon.Set<T>(key, data, expires) ? Task.FromResult(data) : Task.FromResult(res);
            });
        }

        public Task<T> Get<T> (string key)
        {
            return RedisHelper.GetAsync<T>(key);
        }

        public Task<T> GetOrAdd<T> (string key, T data, TimeSpan? expires = null)
        {
            return Task.Run<T>(() =>
            {
                if (RedisCommon.TryGet<T>(key, out T obj))
                {
                    return obj;
                }
                else if (RedisCommon.Add<T>(key, data, expires))
                {
                    return data;
                }
                else
                {
                    return RedisCommon.TryGet<T>(key, out obj) ? obj : default;
                }
            });
        }

        public async Task<bool> Remove (string key)
        {
            return await RedisHelper.DelAsync(key) > 0;
        }

        public Task<bool> Replace<T> (string key, T data, TimeSpan? expires = null)
        {
            return RedisCommon.ReplaceAsync<T>(key, data, expires);
        }

        public Task<bool> Set<T> (string key, T data, TimeSpan? expires = null)
        {
            return RedisCommon.SetAsync<T>(key, data, expires);
        }

        public Task<T> TryRemove<T> (string key)
        {
            return Task.Run<T>(() =>
            {
                if (!RedisCommon.TryGet(key, out T data))
                {
                    return Task.FromResult(default(T));
                }
                else if (RedisHelper.Del(key) > 0)
                {
                    return Task.FromResult(default(T));
                }
                return Task.FromResult(default(T));
            });
        }
        public Task<T> TryUpdate<T> (string key, T data, Func<T, T, T> upFunc, TimeSpan? expires = null)
        {
            return Task.Run<T>(() =>
            {
                if (RedisCommon.TryGet<T>(key, out T res))
                {
                    data = upFunc(res, data);
                    if (data == null || data.Equals(res))
                    {
                        return res;
                    }
                }
                return RedisCommon.Set<T>(key, data, expires) ? data : res;
            });
        }
    }
}
