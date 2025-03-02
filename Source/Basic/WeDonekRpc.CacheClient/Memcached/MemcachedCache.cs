using System;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using WeDonekRpc.CacheClient.Config;
using WeDonekRpc.CacheClient.Helper;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.CacheClient.Log;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.CacheClient.Memcached
{
    internal class MemcachedCache : ICacheController
    {
        private static MemcachedClient _Client = null;
        private const int _CasSuccessStatusCode = 0;

        public CacheType CacheType => CacheType.Memcached;
        public bool IsEnable => RpcCacheService.CheckIsInit(this.CacheType);
        public MemcachedCache ()
        {
        }
        public static bool InitCache ( MemcachedConfig config )
        {
            if ( config.ServerIp.IsNull() )
            {
                return false;
            }
            try
            {
                MemcachedLogFactory _Log = new MemcachedLogFactory();
                MemcachedClientOptions options = new MemcachedClientOptions
                {
                    Protocol = MemcachedProtocol.Text
                };
                if ( !config.UserName.IsNull() && !config.Pwd.IsNull() )
                {
                    Authentication auth = new Authentication
                    {
                        Type = "PlainTextAuthenticator"
                    };
                    auth.Parameters.Add("zone", string.Empty);
                    auth.Parameters.Add("userName", config.UserName);
                    auth.Parameters.Add("password", config.Pwd);
                }
                MemcachedClientConfiguration mc = new MemcachedClientConfiguration(_Log, options);
                mc.SocketPool.MinPoolSize = config.MinPoolSize;
                mc.SocketPool.MaxPoolSize = config.MaxPoolSize;
                mc.SocketPool.ConnectionTimeout = new TimeSpan(0, 0, config.ConnectionTimeout);
                mc.Transcoder = new MemcachedTranscoder();
                config.ServerIp.ForEach(a =>
                {
                    mc.Servers.Add(CacheHelper.GetServer(a, 11211));
                });
                _Client = new MemcachedClient(_Log, mc);
                return _CheShi();
            }
            catch ( Exception e )
            {
                new ErrorLog(e, "缓存初始错误", "Memcached")
                {
                    LogContent = config.ToJson()
                }.Save();
                return false;
            }
        }
        private static bool _CheShi ()
        {
            string name = Guid.NewGuid().ToString("N");
            return _Client.Store(StoreMode.Set, name, name, DateTime.Now.AddMinutes(2));
        }

        #region 获取
        public bool TryGet<T> ( string key, out T data )
        {
            return _Client.TryGet<T>(key, out data);
        }
        public bool TryGet<T> ( string key, out T data, out ulong cas )
        {
            if ( _Client.TryGetWithCas<T>(key, out CasResult<T> res) && res.StatusCode == _CasSuccessStatusCode )
            {
                cas = res.Cas;
                data = res.Result;
                return true;
            }
            cas = 0;
            data = default;
            return false;
        }
        #endregion

        #region 设置
        public bool Set<T> ( string key, T data, TimeSpan? expires = null )
        {
            expires = CacheTimeConfig.FormatCacheTime(key, expires);
            if ( expires.HasValue )
            {
                return _Client.Store(StoreMode.Set, key, data, expires.Value);
            }
            return _Client.Store(StoreMode.Set, key, data);
        }
        public bool Set<T> ( string key, T data, DateTime expires )
        {
            return _Client.Store(StoreMode.Set, key, data, expires);
        }
        public bool Set<T> ( string key, T data, ref ulong cas, DateTime expires )
        {
            CasResult<bool> result = _Client.Cas(StoreMode.Set, key, data, expires, cas);
            cas = result.Cas;
            return result.Result;
        }
        public bool Set<T> ( string key, T data, ref ulong cas, TimeSpan? expires = null )
        {
            expires = CacheTimeConfig.FormatCacheTime(key, expires);
            CasResult<bool> result = expires.HasValue ? _Client.Cas(StoreMode.Set, key, data, expires.Value, cas) : _Client.Cas(StoreMode.Set, key, data, cas);
            if ( result.Result )
            {
                cas = result.Cas;
                return true;
            }
            return false;
        }

        #endregion
        #region 添加
        public bool Add<T> ( string key, T data, TimeSpan? expires = null )
        {
            expires = CacheTimeConfig.FormatCacheTime(key, expires);
            if ( expires.HasValue )
            {
                return _Client.Store(StoreMode.Add, key, data, expires.Value);
            }
            return _Client.Store(StoreMode.Add, key, data);
        }

        public bool CasAdd<T> ( string key, T data, TimeSpan? expires = null )
        {
            expires = CacheTimeConfig.FormatCacheTime(key, expires);
            if ( expires.HasValue )
            {
                return _Client.Cas(StoreMode.Add, key, data, expires.Value, 0).Result;
            }
            else
            {
                return _Client.Cas(StoreMode.Add, key, data, 0).Result;
            }
        }

        #endregion

        #region 替换
        public bool Replace<T> ( string key, T data, TimeSpan? expires = null )
        {
            expires = CacheTimeConfig.FormatCacheTime(key, expires);
            if ( expires.HasValue )
            {
                return _Client.Store(StoreMode.Replace, key, data, expires.Value);
            }
            return _Client.Store(StoreMode.Replace, key, data);
        }
        public bool Replace<T> ( string key, T data, DateTime expires )
        {
            return _Client.Store(StoreMode.Replace, key, data, expires);
        }
        public bool Replace<T> ( string key, T data, ref ulong cas, DateTime expires )
        {
            CasResult<bool> result = _Client.Cas(StoreMode.Replace, key, data, expires, cas);
            cas = result.Cas;
            return result.Result;
        }
        public bool Replace<T> ( string key, T data, ref ulong cas, TimeSpan? expires = null )
        {
            expires = CacheTimeConfig.FormatCacheTime(key, expires);
            CasResult<bool> result = expires.HasValue
                ? _Client.Cas(StoreMode.Replace, key, data, expires.Value, cas)
                : _Client.Cas(StoreMode.Replace, key, data, cas);
            cas = result.Cas;
            return result.Result;
        }
        #endregion


        #region 获取或添加


        public T GetOrAdd<T> ( string key, T data, TimeSpan? expires = null )
        {
            if ( this.TryGet(key, out data) )
            {
                return data;
            }
            else if ( this.Add(key, data, expires) )
            {
                return data;
            }
            else if ( this.TryGet(key, out data) )
            {
                return data;
            }
            return default;
        }
        #endregion
        #region 移除
        public bool TryRemove<T> ( string key, Func<T, bool> func, out T data )
        {
            if ( _Client.TryGet(key, out data) )
            {
                return !func(data) || _Client.Remove(key);
            }
            data = default;
            return false;
        }
        public bool TryRemove<T> ( string key, out T data )
        {
            if ( _Client.TryGet(key, out data) )
            {
                if ( !_Client.Remove(key) )
                {
                    data = default;
                    return false;
                }
                return true;
            }
            data = default;
            return false;
        }
        public bool Remove ( string key )
        {
            return _Client.Remove(key);
        }
        #endregion

        #region 添加或修改

        public T AddOrUpdate<T> ( string key, T data, Func<T, T, T> upFunc, TimeSpan? expires = null )
        {
            StoreMode mode = StoreMode.Add;
            if ( this.TryGet(key, out T res) )
            {
                data = upFunc(res, data);
                if ( data == null || data.Equals(res) )
                {
                    return res;
                }
                mode = StoreMode.Set;
            }
            expires = CacheTimeConfig.FormatCacheTime(key, expires);
            if ( expires.HasValue )
            {
                return _Client.Store(mode, key, data, expires.Value) ? data : res;
            }
            return _Client.Store(mode, key, data) ? data : res;
        }
        #endregion

        public T TryUpdate<T> ( string key, T data, Func<T, T, T> upFunc, TimeSpan? expires = null )
        {
            if ( this.TryGet(key, out T res) )
            {
                data = upFunc(res, data);
                if ( data == null || data.Equals(res) )
                {
                    return res;
                }
            }
            else
            {
                return default;
            }
            expires = CacheTimeConfig.FormatCacheTime(key, expires);
            if ( expires.HasValue )
            {
                return _Client.Store(StoreMode.Replace, key, data, expires.Value) ? data : res;
            }
            return _Client.Store(StoreMode.Replace, key, data) ? data : res;
        }
        public bool TryUpdate<T> ( string key, Func<T, T> upFunc, out T data, TimeSpan? expires = null )
        {
            if ( this.TryGet(key, out data) )
            {
                T res = upFunc(data);
                if ( res == null || res.Equals(data) )
                {
                    return true;
                }
                data = res;
            }
            else
            {
                return true;
            }
            expires = CacheTimeConfig.FormatCacheTime(key, expires);
            if ( expires.HasValue )
            {
                return _Client.Store(StoreMode.Replace, key, data, expires.Value);
            }
            return _Client.Store(StoreMode.Replace, key, data);
        }
        public static void Close ()
        {
            _Client?.Dispose();
        }

        public bool TryRemove<T> ( string key, Func<T, bool> func )
        {
            if ( _Client.TryGet(key, out T data) )
            {
                return !func(data) || _Client.Remove(key);
            }
            return false;
        }
    }
}
