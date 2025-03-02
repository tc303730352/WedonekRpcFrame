using System;
using WeDonekRpc.CacheClient.Interface;

namespace WeDonekRpc.CacheClient.Redis
{
    internal class RedisBasicCache : ICacheController
    {
        public bool IsEnable
        {
            get => RpcCacheService.CheckIsInit(this.CacheType);
        }

        public string FormatKey (string key)
        {
            return RpcCacheService.FormatKey(key);
        }

        public CacheType CacheType { get; }

        public RedisBasicCache ()
        {
            this.CacheType = CacheType.Redis;
        }
        public T AddOrUpdate<T> (string key, T data, Func<T, T, T> upFunc, TimeSpan? expires = null)
        {
            if (RedisCommon.TryGet<T>(key, out T res))
            {
                data = upFunc(res, data);
                if (data == null || data.Equals(res))
                {
                    return res;
                }
            }
            return RedisCommon.Set<T>(key, data, expires) ? data : res;
        }

        public T GetOrAdd<T> (string key, T data, TimeSpan? expires = null)
        {
            if (RedisCommon.TryGet<T>(key, out T obj))
            {
                return obj;
            }
            else if (RedisCommon.Add<T>(key, data, expires))
            {
                return data;
            }
            else
            {
                return RedisCommon.TryGet<T>(key, out obj) ? obj : default;
            }
        }

        public bool Remove (string key)
        {
            return RedisCommon.Remove(key);
        }
        public bool Set<T> (string key, T data, TimeSpan? expires = null)
        {
            return RedisCommon.Set<T>(key, data, expires);
        }
        public bool Set<T> (string key, T data, DateTime expires)
        {
            return RedisCommon.Set<T>(key, data, expires - DateTime.Now);
        }
        public bool TryGet<T> (string key, out T data)
        {
            return RedisCommon.TryGet(key, out data);
        }
        public bool TryRemove<T> (string key, Func<T, bool> func, out T data)
        {
            if (!RedisCommon.TryGet(key, out data))
            {
                return false;
            }
            else if (!func(data))
            {
                return true;
            }
            return RedisCommon.Remove(key);
        }
        public bool TryRemove<T> (string key, out T data)
        {
            return RedisCommon.TryGet(key, out data) && RedisCommon.Remove(key);
        }

        public bool Replace<T> (string key, T data, TimeSpan? expires = null)
        {
            return RedisCommon.Replace<T>(key, data, expires);
        }

        public bool Add<T> (string key, T data, TimeSpan? expires = null)
        {
            return RedisCommon.Add<T>(key, data, expires);
        }

        public bool TryUpdate<T> (string key, Func<T, T> upFunc, out T data, TimeSpan? expires = null)
        {
            if (RedisCommon.TryGet<T>(key, out data))
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
                return false;
            }
            return RedisCommon.Replace<T>(key, data, expires);
        }

        public T TryUpdate<T> (string key, T data, Func<T, T, T> upFunc, TimeSpan? expires = null)
        {
            if (RedisCommon.TryGet<T>(key, out T res))
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
            return RedisCommon.Replace<T>(key, data, expires) ? data : res;
        }

        public bool TryRemove<T> (string key, Func<T, bool> func)
        {
            if (!RedisCommon.TryGet<T>(key, out T res))
            {
                return false;
            }
            else if (func(res))
            {
                return RedisCommon.Remove(key);
            }
            return false;
        }
    }
}
