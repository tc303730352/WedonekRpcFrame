using System;
using System.Threading.Tasks;

namespace RpcCacheClient.Interface
{
        public interface IAsyncCacheController
        {
                CacheType CacheType { get; }

                Task<bool> Add<T>(string key, T data);
                Task<bool> Add<T>(string key, T data, TimeSpan expiresAt);
                Task<T> AddOrUpdate<T>(string key, T data, Func<T, T, T> upFunc);
                Task<T> AddOrUpdate<T>(string key, T data, Func<T, T, T> upFunc, TimeSpan expiresAt);
                Task<T> Get<T>(string key);
                Task<T> GetOrAdd<T>(string key, T data);
                Task<T> GetOrAdd<T>(string key, T data, TimeSpan expiresAt);
                Task<bool> Remove(string key);
                Task<bool> Replace<T>(string key, T data);
                Task<bool> Replace<T>(string key, T data, TimeSpan expiresAt);
                Task<bool> Set<T>(string key, T data);
                Task<bool> Set<T>(string key, T data, TimeSpan expiresAt);
                Task<T> TryRemove<T>(string key);
                Task<T> TryUpdate<T>(string key, T data, Func<T, T, T> upFunc);
                Task<T> TryUpdate<T>(string key, T data, Func<T, T, T> upFunc, TimeSpan expiresAt);
        }
}