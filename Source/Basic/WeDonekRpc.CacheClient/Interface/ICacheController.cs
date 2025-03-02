using System;

namespace WeDonekRpc.CacheClient.Interface
{
    public enum CacheType
    {
        Memcached = 0,
        Redis = 1,
        Local = 2
    }
    /// <summary>
    /// 缓存组件
    /// </summary>
    public interface ICacheController
    {
        /// <summary>
        /// 缓存类型
        /// </summary>
        CacheType CacheType { get; }
        /// <summary>
        /// 是否启用
        /// </summary>
        bool IsEnable{ get; }
        /// <summary>
        /// 添加或更新
        /// </summary>
        /// <typeparam name="T">缓存的对象</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="data">缓存数据</param>
        /// <param name="upFunc">更新比对(返回最新的数据)</param>
        /// <param name="expiresAt">过期时间</param>
        /// <returns>最新数据</returns>
        T AddOrUpdate<T>(string key, T data, Func<T, T, T> upFunc, TimeSpan? expires=null);

        T GetOrAdd<T>(string key, T data, TimeSpan? expires=null);
        bool TryUpdate<T>(string key, Func<T, T> upFunc, out T data, TimeSpan? expires=null);
        bool Remove(string key);
        bool Set<T>(string key, T data, DateTime expires);
        bool Set<T>(string key, T data, TimeSpan? expires = null);
        bool Replace<T>(string key, T data, TimeSpan? expires=null);
        bool Add<T>(string key, T data, TimeSpan? expires=null);
        bool TryGet<T>(string key, out T data);
        bool TryRemove<T>(string key, out T data);
        bool TryRemove<T>(string key, Func<T, bool> func, out T data);
        bool TryRemove<T>(string key, Func<T, bool> func);
        T TryUpdate<T>(string key, T data, Func<T, T, T> upFunc, TimeSpan? expires=null);
    }
}