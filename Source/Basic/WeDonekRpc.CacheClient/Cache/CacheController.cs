using System;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.CacheClient.Memcached;
using WeDonekRpc.CacheClient.Redis;

namespace WeDonekRpc.CacheClient.Cache
{
    internal class CacheController : ICacheController
    {
        public CacheType CacheType { get; private set; }
        private readonly bool _isInit = false;
        private ICacheController _CacheClient = null;

        public bool IsEnable
        {
            get => RpcCacheService.CheckIsInit(this.CacheType);
        }
        protected internal ICacheController CacheClient
        {
            get
            {
                if (this._CacheClient == null)
                {
                    this._CacheClient = this._CreateCache();
                }
                return this._CacheClient;
            }
        }
        public CacheController ()
        {
            this._isInit = false;
        }
        protected CacheController (CacheType cacheType)
        {
            this._isInit = true;
            this.CacheType = cacheType;
        }
        private ICacheController _CreateCache ()
        {
            if (!this._isInit)
            {
                this.CacheType = RpcCacheService.DefCacheType;
            }
            if (this.CacheType == CacheType.Local)
            {
                return new Local.LocalCache();
            }
            return this.CacheType == CacheType.Memcached ? new MemcachedCache() : new RedisBasicCache();
        }

        public T AddOrUpdate<T> (string key, T data, Func<T, T, T> upFunc, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            return this.CacheClient.AddOrUpdate<T>(key, data, upFunc, expires);
        }

        public T GetOrAdd<T> (string key, T data, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            return this.CacheClient.GetOrAdd<T>(key, data, expires);
        }
        public bool Remove (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this.CacheClient.Remove(key);
        }
        public bool Set<T> (string key, T data, DateTime expires)
        {
            key = RpcCacheService.FormatKey(key);
            return this.CacheClient.Set<T>(key, data, expires);
        }
        public bool Set<T> (string key, T data, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            return this.CacheClient.Set<T>(key, data, expires);
        }

        public bool TryGet<T> (string key, out T data)
        {
            key = RpcCacheService.FormatKey(key);
            return this.CacheClient.TryGet<T>(key, out data);
        }


        public bool TryRemove<T> (string key, out T data)
        {
            key = RpcCacheService.FormatKey(key);
            return this.CacheClient.TryRemove<T>(key, out data);
        }
        public bool TryRemove<T> (string key, Func<T, bool> func, out T data)
        {
            key = RpcCacheService.FormatKey(key);
            return this.CacheClient.TryRemove<T>(key, func, out data);
        }

        public bool Replace<T> (string key, T data, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            return this.CacheClient.Replace<T>(key, data, expires);
        }

        public bool Add<T> (string key, T data, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            return this.CacheClient.Add<T>(key, data, expires);
        }

        public T TryUpdate<T> (string key, T data, Func<T, T, T> upFunc, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            return this.CacheClient.TryUpdate<T>(key, data, upFunc, expires);
        }

        public bool TryUpdate<T> (string key, Func<T, T> upFunc, out T data, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            return this.CacheClient.TryUpdate<T>(key, upFunc, out data, expires);
        }

        public bool TryRemove<T> (string key, Func<T, bool> func)
        {
            key = RpcCacheService.FormatKey(key);
            return this.CacheClient.TryRemove<T>(key, func);
        }
    }
}
