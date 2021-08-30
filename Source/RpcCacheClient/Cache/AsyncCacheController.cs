using System;
using System.Threading.Tasks;

using RpcCacheClient.Interface;
using RpcCacheClient.Redis;
namespace RpcCacheClient.Cache
{
        public class AsyncCacheController : IAsyncCacheController
        {
                public CacheType CacheType => this._CacheClient.CacheType;
                private readonly IAsyncCacheController _CacheClient = null;

                protected internal IAsyncCacheController CacheClient => this._CacheClient;

                public AsyncCacheController(CacheType cacheType)
                {
                        if (cacheType == CacheType.Redis)
                        {
                                this._CacheClient = new AsyncRedisCache();
                        }
                }

                public async Task<bool> Add<T>(string key, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._CacheClient.Add<T>(key, data);
                }

                public async Task<bool> Add<T>(string key, T data, TimeSpan expiresAt)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._CacheClient.Add<T>(key, data, expiresAt);
                }
                public async Task<T> AddOrUpdate<T>(string key, T data, Func<T, T, T> upFunc)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._CacheClient.AddOrUpdate<T>(key, data, upFunc);
                }

                public async Task<T> AddOrUpdate<T>(string key, T data, Func<T, T, T> upFunc, TimeSpan expiresAt)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._CacheClient.AddOrUpdate<T>(key, data, upFunc, expiresAt);
                }
                public async Task<T> GetOrAdd<T>(string key, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._CacheClient.GetOrAdd<T>(key, data);
                }

                public async Task<T> GetOrAdd<T>(string key, T data, TimeSpan expiresAt)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._CacheClient.GetOrAdd<T>(key, data, expiresAt);
                }
                public async Task<bool> Remove(string key)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._CacheClient.Remove(key);
                }
                public async Task<bool> Replace<T>(string key, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._CacheClient.Replace<T>(key, data);
                }

                public async Task<bool> Replace<T>(string key, T data, TimeSpan expiresAt)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._CacheClient.Replace<T>(key, data, expiresAt);
                }
                public async Task<bool> Set<T>(string key, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._CacheClient.Set<T>(key, data);
                }

                public async Task<bool> Set<T>(string key, T data, TimeSpan expiresAt)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._CacheClient.Set<T>(key, data, expiresAt);
                }
                public async Task<T> TryRemove<T>(string key)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._CacheClient.TryRemove<T>(key);
                }
                public async Task<T> TryUpdate<T>(string key, T data, Func<T, T, T> upFunc)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._CacheClient.TryUpdate<T>(key, data, upFunc);
                }

                public async Task<T> TryUpdate<T>(string key, T data, Func<T, T, T> upFunc, TimeSpan expiresAt)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._CacheClient.TryUpdate<T>(key, data, upFunc, expiresAt);
                }

                public async Task<T> Get<T>(string key)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._CacheClient.Get<T>(key);
                }
        }
}
