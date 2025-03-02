using System;
using System.Threading.Tasks;

namespace WeDonekRpc.CacheClient.Interface
{
    public interface IAsyncCacheController
    {
        CacheType CacheType { get; }

        Task<bool> Add<T>(string key, T data, TimeSpan? expires=null);
        Task<T> AddOrUpdate<T>(string key, T data, Func<T, T, T> upFunc, TimeSpan? expires=null);
        Task<T> Get<T>(string key);
        Task<T> GetOrAdd<T>(string key, T data, TimeSpan? expires=null);
        Task<bool> Remove(string key);
        Task<bool> Replace<T>(string key, T data, TimeSpan? expires=null);
        Task<bool> Set<T>(string key, T data, TimeSpan? expires=null);
        Task<T> TryRemove<T>(string key);
        Task<T> TryUpdate<T>(string key, T data, Func<T, T, T> upFunc, TimeSpan? expires=null);
    }
}