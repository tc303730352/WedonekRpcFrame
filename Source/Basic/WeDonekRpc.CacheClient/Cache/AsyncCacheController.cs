using System;
using System.Threading.Tasks;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.CacheClient.Redis;
namespace WeDonekRpc.CacheClient.Cache
{
    public class AsyncCacheController : IAsyncCacheController
    {
        public CacheType CacheType { get; }

        private readonly IAsyncCacheController _CacheClient = null;

        protected internal IAsyncCacheController CacheClient => this._CacheClient;

        public AsyncCacheController (CacheType cacheType)
        {
            this.CacheType = cacheType;
            if (cacheType == CacheType.Redis)
            {
                this._CacheClient = new AsyncRedisCache();
            }
        }

        public async Task<bool> Add<T> (string key, T data, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            return await this._CacheClient.Add<T>(key, data, expires);
        }

        public async Task<T> AddOrUpdate<T> (string key, T data, Func<T, T, T> upFunc, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            return await this._CacheClient.AddOrUpdate<T>(key, data, upFunc, expires);
        }

        public async Task<T> GetOrAdd<T> (string key, T data, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            return await this._CacheClient.GetOrAdd<T>(key, data, expires);
        }
        public async Task<bool> Remove (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return await this._CacheClient.Remove(key);
        }

        public async Task<bool> Replace<T> (string key, T data, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            return await this._CacheClient.Replace<T>(key, data, expires);
        }

        public async Task<bool> Set<T> (string key, T data, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            return await this._CacheClient.Set<T>(key, data, expires);
        }
        public async Task<T> TryRemove<T> (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return await this._CacheClient.TryRemove<T>(key);
        }

        public async Task<T> TryUpdate<T> (string key, T data, Func<T, T, T> upFunc, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            return await this._CacheClient.TryUpdate<T>(key, data, upFunc, expires);
        }

        public async Task<T> Get<T> (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return await this._CacheClient.Get<T>(key);
        }
    }
}
