using System;

namespace WeDonekRpc.CacheClient.Interface
{
    public interface ICasMemcachedCache
    {
        bool IsEnable { get; }

        bool Add<T> ( string key, T data, TimeSpan? expires = null );
        T AddOrUpdate<T> ( string key, T data, Func<T, T, T> upFunc, TimeSpan? expires = null );
        T GetOrAdd<T> ( string key, T data, TimeSpan? expires = null );
        bool Remove ( string key );
        bool Replace<T> ( string key, T data, TimeSpan? expires = null );
        bool Set<T> ( string key, T data, TimeSpan? expires = null );
        bool Set<T> ( string key, T data, DateTime expires );
        bool TryGet<T> ( string key, out T data, out ulong cas );
        bool TryRemove<T> ( string key, Func<T, bool> func );
        bool TryRemove<T> ( string key, Func<T, bool> func, out T data );
        bool TryRemove<T> ( string key, out T data );
        bool TryUpdate<T> ( string key, Func<T, T> upFunc, out T data, TimeSpan? expires = null );
        T TryUpdate<T> ( string key, T data, Func<T, T, T> upFunc, TimeSpan? expires = null );
        T TryUpdate<T> ( string key, T data, Func<T, T, T> upFunc, DateTime expires );
    }
}