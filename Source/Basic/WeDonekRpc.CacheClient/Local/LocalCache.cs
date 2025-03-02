using System;
using Microsoft.Extensions.Caching.Memory;
using WeDonekRpc.CacheClient.Interface;
namespace WeDonekRpc.CacheClient.Local
{
    internal class LocalCache : ILocalCacheController
    {
        public CacheType CacheType => CacheType.Local;
        private static readonly MemoryCache _Client = null;
        public bool IsEnable
        {
            get;
        } = true;
        static LocalCache ()
        {
            MemoryCacheOptions options = new MemoryCacheOptions
            {
                CompactionPercentage = 0.2,
                ExpirationScanFrequency = new TimeSpan(0, 0, 10)
            };
            _Client = new MemoryCache(options);
        }

        public bool Add<T> (string key, T data, TimeSpan? expires = null)
        {
            expires = Config.CacheTimeConfig.FormatCacheTime(key, expires);
            if (!expires.HasValue)
            {
                return this._Add(key, data);
            }
            ICacheEntry cache = _Client.CreateEntry(key);
            _ = cache.SetValue(data);
            _ = cache.SetAbsoluteExpiration(expires.Value);
            return true;
        }

        private bool _Add<T> (string key, T data)
        {
            ICacheEntry cache = _Client.CreateEntry(key);
            _ = cache.SetValue(data);
            return true;
        }

        private T _AddOrUpdate<T> (string key, T data, Func<T, T, T> upFunc)
        {
            if (this.TryGet(key, out T res))
            {
                T up = upFunc(res, data);
                if (up == null || up.Equals(res))
                {
                    return res;
                }
                _ = _Client.Set(key, up);
                return data;
            }
            return this._Add(key, data) ? data : default;
        }

        public T AddOrUpdate<T> (string key, T data, Func<T, T, T> upFunc, TimeSpan? expires = null)
        {
            expires = Config.CacheTimeConfig.FormatCacheTime(key, expires);
            if (!expires.HasValue)
            {
                return this._AddOrUpdate(key, data, upFunc);
            }
            if (this.TryGet(key, out T res))
            {
                T up = upFunc(res, data);
                if (up == null || up.Equals(res))
                {
                    return res;
                }
                _ = _Client.Set(key, up, expires.Value);
                return data;
            }
            return this.Add(key, data, expires.Value) ? data : res;
        }


        public T GetOrAdd<T> (string key, T data, TimeSpan? expires = null)
        {
            expires = Config.CacheTimeConfig.FormatCacheTime(key, expires);
            return _Client.GetOrCreate(key, (a) =>
            {
                if (expires.HasValue)
                {
                    _ = a.SetAbsoluteExpiration(expires.Value);
                }
                return data;
            });
        }

        public bool Remove (string key)
        {
            _Client.Remove(key);
            return true;
        }

        public bool Replace<T> (string key, T data, TimeSpan? expires = null)
        {
            if (!_Client.TryGetValue(key, out _))
            {
                return false;
            }
            expires = Config.CacheTimeConfig.FormatCacheTime(key, expires);
            if (expires.HasValue)
            {
                _ = _Client.Set(key, data, expires.Value);
            }
            else
            {
                _ = _Client.Set(key, data);
            }
            return true;
        }


        public bool Set<T> (string key, T data, DateTime expires)
        {
            _ = _Client.Set(key, data, expires);
            return true;
        }
        public bool Set<T> (string key, T data, TimeSpan? expires = null)
        {
            expires = Config.CacheTimeConfig.FormatCacheTime(key, expires);
            if (expires.HasValue)
            {
                _ = _Client.Set(key, data, expires.Value);
            }
            else
            {
                _ = _Client.Set(key, data);
            }
            return true;
        }
        public bool TryGet<T> (string key, out T data)
        {
            return _Client.TryGetValue(key, out data);
        }

        public bool TryRemove<T> (string key, out T data)
        {
            if (_Client.TryGetValue(key, out data))
            {
                _Client.Remove(key);
                return true;
            }
            return false;
        }

        public bool TryRemove<T> (string key, Func<T, bool> func, out T data)
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

        public bool TryUpdate<T> (string key, Func<T, T> upFunc, out T data, TimeSpan? expires = null)
        {
            if (this.TryGet(key, out data))
            {
                T val = upFunc(data);
                if (val == null || val.Equals(data))
                {
                    return true;
                }
                data = val;
            }
            else
            {
                return false;
            }
            expires = Config.CacheTimeConfig.FormatCacheTime(key, expires);
            if (expires.HasValue)
            {
                _ = _Client.Set(key, data, expires.Value);
            }
            else
            {
                _ = _Client.Set(key, data);
            }
            return true;
        }

        public T TryUpdate<T> (string key, T data, Func<T, T, T> upFunc, TimeSpan? expires = null)
        {
            if (this.TryGet(key, out T res))
            {
                data = upFunc(res, data);
                if (data == null || data.Equals(res))
                {
                    return res;
                }
            }
            else
            {
                return default;
            }
            expires = Config.CacheTimeConfig.FormatCacheTime(key, expires);
            if (expires.HasValue)
            {
                _ = _Client.Set(key, data, expires.Value);
            }
            else
            {
                _ = _Client.Set(key, data);
            }
            return data;
        }
        public static void Close ()
        {
            _Client.Dispose();
        }

        public bool TryRemove<T> (string key, Func<T, bool> func)
        {
            if (_Client.TryGetValue(key, out T data))
            {
                if (func(data))
                {
                    _Client.Remove(key);
                    return true;
                }
            }
            return false;
        }
    }
}
