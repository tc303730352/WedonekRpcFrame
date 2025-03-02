﻿using System;
using WeDonekRpc.CacheClient.Interface;

namespace WeDonekRpc.CacheClient.Local
{
    internal class LocalCacheService : ILocalCacheController
    {
        public CacheType CacheType => CacheType.Local;
        private readonly LocalCache _Cache;
        public bool IsEnable
        {
            get;
        } = true;
        public LocalCacheService ()
        {
            this._Cache = new LocalCache();
        }

        public bool Add<T> (string key, T data, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Cache.Add(key, data, expires);
        }



        public T AddOrUpdate<T> (string key, T data, Func<T, T, T> upFunc, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Cache.AddOrUpdate(key, data, upFunc, expires);
        }

        public T GetOrAdd<T> (string key, T data, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Cache.GetOrAdd(key, data, expires);
        }

        public bool Remove (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Cache.Remove(key);
        }
        public bool Replace<T> (string key, T data, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Cache.Replace(key, data, expires);
        }
        public bool Set<T> (string key, T data, DateTime expires)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Cache.Set(key, data, expires);
        }

        public bool Set<T> (string key, T data, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Cache.Set(key, data, expires);
        }


        public bool TryGet<T> (string key, out T data)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Cache.TryGet(key, out data);
        }

        public bool TryRemove<T> (string key, out T data)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Cache.TryRemove(key, out data);
        }

        public bool TryRemove<T> (string key, Func<T, bool> func, out T data)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Cache.TryRemove(key, func, out data);
        }

        public bool TryRemove<T> (string key, Func<T, bool> func)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Cache.TryRemove(key, func);
        }

        public bool TryUpdate<T> (string key, Func<T, T> upFunc, out T data, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Cache.TryUpdate(key, upFunc, out data, expires);
        }

        public T TryUpdate<T> (string key, T data, Func<T, T, T> upFunc, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Cache.TryUpdate(key, data, upFunc, expires);
        }
    }
}
