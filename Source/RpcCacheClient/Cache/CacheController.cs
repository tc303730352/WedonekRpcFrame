using System;

using RpcCacheClient.Interface;
using RpcCacheClient.Memcached;
using RpcCacheClient.Redis;

namespace RpcCacheClient.Cache
{

        public class CacheController : ICacheController
        {
                public CacheType CacheType => this._CacheClient.CacheType;
                private readonly ICacheController _CacheClient = null;

                protected internal ICacheController CacheClient => this._CacheClient;
                public CacheController() : this(RpcCacheService.DefCacheType, RpcCacheService.RedisDb)
                {

                }
                public CacheController(CacheType cacheType, int db)
                {
                        if (cacheType == CacheType.Local)
                        {
                                this._CacheClient = new Local.LocalCache();
                        }
                        else
                        {
                                this._CacheClient = cacheType == CacheType.Memcached ? new MemcachedCache() : new RedisCache(db);
                        }
                }
                public CacheController(bool isCasModel = false)
                {
                        this._CacheClient = isCasModel ? new CasMemcachedCache() : new MemcachedCache();
                }

                public T AddOrUpdate<T>(string key, T data, Func<T, T, T> upFunc)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._CacheClient.AddOrUpdate<T>(key, data, upFunc);
                }

                public T AddOrUpdate<T>(string key, T data, Func<T, T, T> upFunc, TimeSpan expiresAt)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._CacheClient.AddOrUpdate<T>(key, data, upFunc, expiresAt);
                }


                public T GetOrAdd<T>(string key, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._CacheClient.GetOrAdd<T>(key, data);
                }

                public T GetOrAdd<T>(string key, T data, TimeSpan expiresAt)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._CacheClient.GetOrAdd<T>(key, data, expiresAt);
                }
                public bool Remove(string key)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._CacheClient.Remove(key);
                }

                public bool Set<T>(string key, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._CacheClient.Set<T>(key, data);
                }

                public bool Set<T>(string key, T data, TimeSpan expiresAt)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._CacheClient.Set<T>(key, data, expiresAt);
                }
                public bool Set<T>(string key, T data, DateTime expires)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._CacheClient.Set<T>(key, data, expires);
                }
                public bool TryGet<T>(string key, out T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._CacheClient.TryGet<T>(key, out data);
                }


                public bool TryRemove<T>(string key, out T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._CacheClient.TryRemove<T>(key, out data);
                }
                public bool TryRemove<T>(string key, Func<T, bool> func, out T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._CacheClient.TryRemove<T>(key, func, out data);
                }
                public T TryUpdate<T>(string key, T data, Func<T, T, T> upFunc)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._CacheClient.TryUpdate<T>(key, data, upFunc);
                }

                public bool Replace<T>(string key, T data, TimeSpan expiresAt)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._CacheClient.Replace<T>(key, data, expiresAt);
                }

                public bool Replace<T>(string key, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._CacheClient.Replace<T>(key, data);
                }

                public bool Add<T>(string key, T data, TimeSpan expiresAt)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._CacheClient.Add<T>(key, data, expiresAt);
                }

                public bool Add<T>(string key, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._CacheClient.Add<T>(key, data);
                }

                public T TryUpdate<T>(string key, T data, Func<T, T, T> upFunc, TimeSpan expiresAt)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._CacheClient.TryUpdate<T>(key, data, upFunc, expiresAt);
                }

                public bool TryUpdate<T>(string key, Func<T, T> upFunc, out T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._CacheClient.TryUpdate<T>(key, upFunc, out data);
                }

                public bool TryUpdate<T>(string key, Func<T, T> upFunc, out T data, TimeSpan expiresAt)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._CacheClient.TryUpdate<T>(key, upFunc, out data, expiresAt);
                }
        }
}
