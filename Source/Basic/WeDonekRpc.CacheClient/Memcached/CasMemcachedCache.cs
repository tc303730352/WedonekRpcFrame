using System;
using WeDonekRpc.CacheClient.Interface;

namespace WeDonekRpc.CacheClient.Memcached
{
    internal class CasMemcachedCache : ICasMemcachedCache
    {
        private readonly MemcachedCache _Client = null;

        public bool IsEnable
        {
            get => RpcCacheService.CheckIsInit(CacheType.Memcached);
        }

        public CasMemcachedCache ()
        {
            this._Client = new MemcachedCache();
        }
        public bool TryGet<T> (string key, out T data, out ulong cas)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.TryGet(key, out data, out cas);
        }

        public T AddOrUpdate<T> (string key, T data, Func<T, T, T> upFunc, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            if (this._Client.TryGet<T>(key, out T res, out ulong cas))
            {
                data = upFunc(res, data);
                if (data == null || data.Equals(res))
                {
                    return res;
                }
            }
            if (expires.HasValue)
            {
                return this._Client.Set(key, data, ref cas, expires.Value) ? data : res;
            }
            return this._Client.Set(key, data, ref cas) ? data : res;
        }



        public T TryUpdate<T> (string key, T data, Func<T, T, T> upFunc, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            if (this._Client.TryGet<T>(key, out T res, out ulong cas))
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
            if (expires.HasValue)
            {
                return this._Client.Replace<T>(key, data, ref cas, expires.Value) ? data : res;
            }
            return this._Client.Replace<T>(key, data, ref cas) ? data : res;
        }
        public T TryUpdate<T> (string key, T data, Func<T, T, T> upFunc, DateTime expires)
        {
            key = RpcCacheService.FormatKey(key);
            if (this._Client.TryGet<T>(key, out T res, out ulong cas))
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
            return this._Client.Replace<T>(key, data, ref cas, expires) ? data : res;
        }
        public bool TryRemove<T> (string key, Func<T, bool> func, out T data)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.TryGet<T>(key, out data, out _) && ( !func(data) || this._Client.Remove(key) );
        }
        public bool TryRemove<T> (string key, out T data)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.TryGet<T>(key, out data, out _) && this._Client.Remove(key);
        }



        public T GetOrAdd<T> (string key, T data, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            if (this._Client.TryGet(key, out data, out _))
            {
                return data;
            }
            else if (this.Add(key, data, expires))
            {
                return data;
            }
            else
            {
                return this._Client.TryGet(key, out data, out _) ? data : default;
            }
        }

        public bool Remove (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.Remove(key);
        }

        public bool Set<T> (string key, T data, DateTime expires)
        {
            key = RpcCacheService.FormatKey(key);
            T res = this.TryUpdate(key, data, (a, b) => b, expires);
            return res != null && !res.Equals(default(T));
        }
        public bool Set<T> (string key, T data, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            T res = this.AddOrUpdate(key, data, (a, b) => b, expires);
            return res != null && !res.Equals(default(T));
        }

        public bool Replace<T> (string key, T data, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            T res = this.TryUpdate(key, data, (a, b) => b, expires);
            return res != null && !res.Equals(default(T));
        }

        public bool Add<T> (string key, T data, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            if (expires.HasValue)
            {
                return this._Client.CasAdd(key, data, expires.Value);
            }
            else
            {
                return this._Client.CasAdd(key, data);
            }
        }


        public bool TryUpdate<T> (string key, Func<T, T> upFunc, out T data, TimeSpan? expires = null)
        {
            key = RpcCacheService.FormatKey(key);
            if (this._Client.TryGet<T>(key, out data, out ulong cas))
            {
                T up = upFunc(data);
                if (up == null || up.Equals(data))
                {
                    return true;
                }
                data = up;
            }
            else
            {
                return true;
            }
            if (expires.HasValue)
            {
                return this._Client.Replace<T>(key, data, ref cas, expires.Value);
            }
            return this._Client.Replace<T>(key, data, ref cas);
        }

        public bool TryRemove<T> (string key, Func<T, bool> func)
        {
            key = RpcCacheService.FormatKey(key);
            if (!this._Client.TryGet<T>(key, out T data, out _))
            {
                return false;
            }
            else if (!func(data))
            {
                return false;
            }
            return this._Client.Remove(key);
        }
    }
}
