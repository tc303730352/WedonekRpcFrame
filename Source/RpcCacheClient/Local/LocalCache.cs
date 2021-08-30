using System;

using Microsoft.Extensions.Caching.Memory;

using RpcCacheClient.Interface;
namespace RpcCacheClient.Local
{
        internal class LocalCache : ICacheController
        {
                public CacheType CacheType => CacheType.Local;
                private static readonly MemoryCache _Client = null;

                static LocalCache()
                {
                        MemoryCacheOptions options = new MemoryCacheOptions
                        {
                                CompactionPercentage = 0.2,
                                ExpirationScanFrequency = new TimeSpan(0, 0, 10)
                        };
                        _Client = new MemoryCache(options);
                }

                public bool Add<T>(string key, T data, TimeSpan expiresAt)
                {
                        ICacheEntry cache = _Client.CreateEntry(key);
                        cache.SetValue(data);
                        cache.SetAbsoluteExpiration(expiresAt);
                        return true;
                }

                public bool Add<T>(string key, T data)
                {
                        ICacheEntry cache = _Client.CreateEntry(key);
                        cache.SetValue(data);
                        return true;
                }

                public T AddOrUpdate<T>(string key, T data, Func<T, T, T> upFunc)
                {
                        if (this.TryGet(key, out T res))
                        {
                                T up = upFunc(res, data);
                                if (up == null)
                                {
                                        return res;
                                }
                                _Client.Set(key, data);
                                return data;
                        }
                        return this.Add(key, data) ? data : default;
                }

                public T AddOrUpdate<T>(string key, T data, Func<T, T, T> upFunc, TimeSpan expiresAt)
                {
                        if (this.TryGet(key, out T res))
                        {
                                T up = upFunc(res, data);
                                if (up == null)
                                {
                                        return res;
                                }
                                _Client.Set(key, data, expiresAt);
                                return data;
                        }
                        return this.Add(key, data, expiresAt) ? data : default;
                }

                public T GetOrAdd<T>(string key, T data)
                {
                        return _Client.GetOrCreate(key, (a) =>
                        {
                                return data;
                        });
                }

                public T GetOrAdd<T>(string key, T data, TimeSpan expiresAt)
                {
                        return _Client.GetOrCreate(key, (a) =>
                        {
                                a.SetAbsoluteExpiration(expiresAt);
                                return data;
                        });
                }

                public bool Remove(string key)
                {
                        _Client.Remove(key);
                        return true;
                }

                public bool Replace<T>(string key, T data, TimeSpan expiresAt)
                {
                        if (!_Client.TryGetValue(key, out _))
                        {
                                return false;
                        }
                        _Client.Set(key, data, expiresAt);
                        return true;
                }

                public bool Replace<T>(string key, T data)
                {
                        if (!_Client.TryGetValue(key, out _))
                        {
                                return false;
                        }
                        _Client.Set(key, data);
                        return true;
                }

                public bool Set<T>(string key, T data)
                {
                        _Client.Set(key, data);
                        return true;
                }

                public bool Set<T>(string key, T data, TimeSpan expiresAt)
                {
                        _Client.Set(key, data, expiresAt);
                        return true;
                }

                public bool Set<T>(string key, T data, DateTime expire)
                {
                        _Client.Set(key, data, expire);
                        return true;
                }

                public bool TryGet<T>(string key, out T data)
                {
                        return _Client.TryGetValue(key, out data);
                }

                public bool TryRemove<T>(string key, out T data)
                {
                        if (_Client.TryGetValue(key, out data))
                        {
                                _Client.Remove(key);
                                return true;
                        }
                        return false;
                }

                public bool TryRemove<T>(string key, Func<T, bool> func, out T data)
                {
                        if (_Client.TryGetValue(key, out data))
                        {
                                if (func(data))
                                {
                                        _Client.Remove(key);
                                        return true;
                                }
                        }
                        data = default;
                        return false;
                }

                public T TryUpdate<T>(string key, T data, Func<T, T, T> upFunc)
                {
                        if (this.TryGet(key, out T res))
                        {
                                data = upFunc(res, data);
                                if (data == null)
                                {
                                        return res;
                                }
                        }
                        else
                        {
                                return default;
                        }
                        _Client.Set(key, data);
                        return data;
                }

                public bool TryUpdate<T>(string key, Func<T, T> upFunc, out T data)
                {
                        if (this.TryGet(key, out data))
                        {
                                data = upFunc(data);
                                if (data == null)
                                {
                                        return true;
                                }
                        }
                        else
                        {
                                return false;
                        }
                        _Client.Set(key, data);
                        return true;
                }

                public bool TryUpdate<T>(string key, Func<T, T> upFunc, out T data, TimeSpan expiresAt)
                {
                        if (this.TryGet(key, out data))
                        {
                                data = upFunc(data);
                                if (data == null)
                                {
                                        return true;
                                }
                        }
                        else
                        {
                                return false;
                        }
                        _Client.Set(key, data, expiresAt);
                        return true;
                }

                public T TryUpdate<T>(string key, T data, Func<T, T, T> upFunc, TimeSpan expiresAt)
                {
                        if (this.TryGet(key, out T res))
                        {
                                data = upFunc(res, data);
                                if (data == null)
                                {
                                        return res;
                                }
                        }
                        else
                        {
                                return default;
                        }
                        _Client.Set(key, data, expiresAt);
                        return data;
                }
                public static void Close()
                {
                        _Client.Dispose();
                }
        }
}
