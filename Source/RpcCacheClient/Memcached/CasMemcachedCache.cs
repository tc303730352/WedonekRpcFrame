using System;

using RpcCacheClient.Interface;

namespace RpcCacheClient.Memcached
{
        internal class CasMemcachedCache : ICacheController
        {
                private readonly MemcachedCache _Client = null;


                public CacheType CacheType { get; }

                public CasMemcachedCache()
                {
                        this.CacheType = CacheType.Memcached;
                        this._Client = new MemcachedCache();
                }
                public bool TryGet<T>(string key, out T data)
                {
                        return this._Client.TryGet(key, out data, out _);
                }
                public T AddOrUpdate<T>(string key, T data, Func<T, T, T> upFunc)
                {
                        if (this._Client.TryGet<T>(key, out T res, out ulong cas))
                        {
                                data = upFunc(res, data);
                                if (data == null)
                                {
                                        return res;
                                }
                        }
                        return this._Client.Set(key, data, ref cas) ? data : default;
                }

                public T AddOrUpdate<T>(string key, T data, Func<T, T, T> upFunc, TimeSpan expiresAt)
                {
                        if (this._Client.TryGet<T>(key, out T res, out ulong cas))
                        {
                                data = upFunc(res, data);
                                if (data == null)
                                {
                                        return res;
                                }
                        }
                        return this._Client.Set(key, data, ref cas, expiresAt) ? data : default;
                }

                public T TryUpdate<T>(string key, T data, Func<T, T, T> upFunc)
                {
                        if (this._Client.TryGet<T>(key, out T res, out ulong cas))
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
                        return this._Client.Replace<T>(key, data, ref cas) ? data : res;
                }

                public T TryUpdate<T>(string key, T data, Func<T, T, T> upFunc, TimeSpan expiresAt)
                {
                        if (this._Client.TryGet<T>(key, out T res, out ulong cas))
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
                        return this._Client.Replace<T>(key, data, ref cas, expiresAt) ? data : res;
                }
                public bool TryRemove<T>(string key, Func<T, bool> func, out T data)
                {
                        return this._Client.TryGet<T>(key, out data, out _) && (!func(data) || this._Client.Remove(key));
                }
                public bool TryRemove<T>(string key, out T data)
                {
                        return this._Client.TryGet<T>(key, out data, out _) && this._Client.Remove(key);
                }

                public T GetOrAdd<T>(string key, T data)
                {
                        if (this._Client.TryGet(key, out data, out _))
                        {
                                return data;
                        }
                        else if (this._Client.CasAdd<T>(key, data))
                        {
                                return data;
                        }
                        else
                        {
                                return this._Client.TryGet(key, out data, out _) ? data : default;
                        }
                }

                public T GetOrAdd<T>(string key, T data, TimeSpan expiresAt)
                {
                        if (this._Client.TryGet(key, out data, out _))
                        {
                                return data;
                        }
                        else if (this._Client.CasAdd<T>(key, data, expiresAt))
                        {
                                return data;
                        }
                        else
                        {
                                return this._Client.TryGet(key, out data, out _) ? data : default;
                        }
                }

                public bool Remove(string key)
                {
                        return this._Client.Remove(key);
                }

                public bool Set<T>(string key, T data)
                {
                        T res = this.AddOrUpdate(key, data, (a, b) => b);
                        return res != null && !res.Equals(default(T));
                }
                public bool Set<T>(string key, T data, DateTime expires)
                {
                        T res = this.AddOrUpdate(key, data, (a, b) => b, expires - DateTime.Now);
                        return res != null && !res.Equals(default(T));
                }
                public bool Set<T>(string key, T data, TimeSpan expiresAt)
                {
                        T res = this.AddOrUpdate(key, data, (a, b) => b, expiresAt);
                        return res != null && !res.Equals(default(T));
                }

                public bool Replace<T>(string key, T data, TimeSpan expiresAt)
                {
                        T res = this.TryUpdate(key, data, (a, b) => b, expiresAt);
                        return res != null && !res.Equals(default(T));
                }

                public bool Replace<T>(string key, T data)
                {
                        T res = this.TryUpdate(key, data, (a, b) => b);
                        return res != null && !res.Equals(default(T));
                }

                public bool Add<T>(string key, T data, TimeSpan expiresAt)
                {
                        return this._Client.CasAdd(key, data, expiresAt);
                }

                public bool Add<T>(string key, T data)
                {
                        return this._Client.CasAdd(key, data);
                }

                public bool TryUpdate<T>(string key, Func<T, T> upFunc, out T data)
                {
                        if (this._Client.TryGet<T>(key, out data, out ulong cas))
                        {
                                T up = upFunc(data);
                                if (up == null)
                                {
                                        return true;
                                }
                        }
                        else
                        {
                                return false;
                        }
                        return this._Client.Replace<T>(key, data, ref cas);
                }

                public bool TryUpdate<T>(string key, Func<T, T> upFunc, out T data, TimeSpan expiresAt)
                {
                        if (this._Client.TryGet<T>(key, out data, out ulong cas))
                        {
                                T up = upFunc(data);
                                if (up == null)
                                {
                                        return true;
                                }
                        }
                        else
                        {
                                return true;
                        }
                        return this._Client.Replace<T>(key, data, ref cas, expiresAt);
                }
        }
}
