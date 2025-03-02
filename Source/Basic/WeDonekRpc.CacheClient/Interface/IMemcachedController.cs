using System;

namespace WeDonekRpc.CacheClient.Interface
{
    public interface IMemcachedController : ICacheController
    {
        bool CasAdd<T> (string key, T data, TimeSpan? expires = null);


        bool Replace<T> (string key, T data, ref ulong cas, TimeSpan? expires = null);

        bool Set<T> (string key, T data, ref ulong cas, TimeSpan? expires = null);
        bool TryGet<T> (string key, out T data, out ulong cas);


    }
}