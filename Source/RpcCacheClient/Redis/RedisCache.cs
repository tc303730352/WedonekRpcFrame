using System;

using RpcCacheClient.Interface;

using StackExchange.Redis;

namespace RpcCacheClient.Redis
{
        internal class RedisCache : IRedisCacheController
        {
                protected IDatabase _DataBase = null;
                public bool IsSupportBatch { get; }

                public bool IsSupportTran { get; }

                public CacheType CacheType { get; }

                public RedisCache(int db)
                {
                        this.CacheType = CacheType.Redis;
                        this._DataBase = RedisHelper.GetClient(db);
                        this.IsSupportBatch = false;
                        this.IsSupportTran = false;
                }

                public T AddOrUpdate<T>(string key, T data, Func<T, T, T> upFunc)
                {
                        if (SyncRedisHelper.TryGet<T>(this._DataBase, key, out T res))
                        {
                                data = upFunc(res, data);
                                if (data == null)
                                {
                                        return res;
                                }
                        }
                        return SyncRedisHelper.Set<T>(this._DataBase, key, data) ? data : default;
                }

                public T AddOrUpdate<T>(string key, T data, Func<T, T, T> upFunc, TimeSpan expiresAt)
                {
                        if (SyncRedisHelper.TryGet<T>(this._DataBase, key, out T res))
                        {
                                data = upFunc(res, data);
                                if (data == null)
                                {
                                        return res;
                                }
                        }
                        return SyncRedisHelper.Set<T>(this._DataBase, key, data, expiresAt) ? res : default;
                }

                public T GetOrAdd<T>(string key, T data)
                {
                        return SyncRedisHelper.TryGet(this._DataBase, key, out T obj)
                                ? obj
                                : SyncRedisHelper.Set<T>(this._DataBase, key, data) ? data : SyncRedisHelper.TryGet<T>(this._DataBase, key, out obj) ? obj : default;
                }

                public T GetOrAdd<T>(string key, T data, TimeSpan expiresAt)
                {
                        if (SyncRedisHelper.TryGet<T>(this._DataBase, key, out T obj))
                        {
                                return obj;
                        }
                        else if (SyncRedisHelper.Set<T>(this._DataBase, key, data, expiresAt))
                        {
                                return data;
                        }
                        else
                        {
                                return SyncRedisHelper.TryGet<T>(this._DataBase, key, out obj) ? obj : default;
                        }
                }

                public bool Remove(string key)
                {
                        return SyncRedisHelper.Remove(this._DataBase, key);
                }

                public bool Set<T>(string key, T data)
                {
                        return SyncRedisHelper.Set<T>(this._DataBase, key, data);
                }

                public bool Set<T>(string key, T data, TimeSpan expiresAt)
                {
                        return SyncRedisHelper.Set<T>(this._DataBase, key, data, expiresAt);
                }
                public bool Set<T>(string key, T data, DateTime expires)
                {
                        return SyncRedisHelper.Set<T>(this._DataBase, key, data, expires - DateTime.Now);
                }
                public bool TryGet<T>(string key, out T data)
                {
                        return SyncRedisHelper.TryGet(this._DataBase, key, out data);
                }
                public bool TryRemove<T>(string key, Func<T, bool> func, out T data)
                {
                        if (!SyncRedisHelper.TryGet(this._DataBase, key, out data))
                        {
                                return false;
                        }
                        else if (!func(data))
                        {
                                return true;
                        }
                        return SyncRedisHelper.Remove(this._DataBase, key);
                }
                public bool TryRemove<T>(string key, out T data)
                {
                        return SyncRedisHelper.TryGet(this._DataBase, key, out data) && SyncRedisHelper.Remove(this._DataBase, key);
                }

                public T TryUpdate<T>(string key, T data, Func<T, T, T> upFunc)
                {
                        if (SyncRedisHelper.TryGet<T>(this._DataBase, key, out T res))
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
                        return SyncRedisHelper.Replace<T>(this._DataBase, key, data) ? data : res;
                }

                public bool Replace<T>(string key, T data, TimeSpan expiresAt)
                {
                        return SyncRedisHelper.Replace<T>(this._DataBase, key, data, expiresAt);
                }

                public bool Replace<T>(string key, T data)
                {
                        return SyncRedisHelper.Replace<T>(this._DataBase, key, data);
                }

                public bool Add<T>(string key, T data, TimeSpan expiresAt)
                {
                        return SyncRedisHelper.Add<T>(this._DataBase, key, data, expiresAt);
                }

                public bool Add<T>(string key, T data)
                {
                        return SyncRedisHelper.Add<T>(this._DataBase, key, data);
                }
                public bool TryUpdate<T>(string key, Func<T, T> upFunc, out T data)
                {
                        if (SyncRedisHelper.TryGet<T>(this._DataBase, key, out data))
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
                        return SyncRedisHelper.Replace<T>(this._DataBase, key, data);
                }

                public bool TryUpdate<T>(string key, Func<T, T> upFunc, out T data, TimeSpan expiresAt)
                {
                        if (SyncRedisHelper.TryGet<T>(this._DataBase, key, out data))
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
                        return SyncRedisHelper.Replace<T>(this._DataBase, key, data, expiresAt);
                }

                public T TryUpdate<T>(string key, T data, Func<T, T, T> upFunc, TimeSpan expiresAt)
                {
                        if (SyncRedisHelper.TryGet<T>(this._DataBase, key, out T res))
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
                        return SyncRedisHelper.Replace<T>(this._DataBase, key, data, expiresAt) ? data : res;
                }

                public bool Add<T>(string key, string colname, T data)
                {
                        return SyncRedisHelper.Add<T>(this._DataBase, key, colname, data);
                }
                public bool Set<T>(string key, string colname, T data)
                {
                        return SyncRedisHelper.Set<T>(this._DataBase, key, colname, data);
                }
                public bool Replace<T>(string key, string colname, T data)
                {
                        return SyncRedisHelper.Replace<T>(this._DataBase, key, colname, data);
                }
                public bool Remove(string key, string colname)
                {
                        return SyncRedisHelper.Remove(this._DataBase, key, colname);
                }

                public bool TryGet<T>(string key, string colname, out T data)
                {
                        return SyncRedisHelper.TryGet(this._DataBase, key, colname, out data);
                }

                public bool TryGet<T>(string key, out T[] data)
                {
                        return SyncRedisHelper.TryGet(this._DataBase, key, out data);
                }

                public long Remove(string key, string[] colname)
                {
                        return SyncRedisHelper.Remove(this._DataBase, key, colname);
                }

                public string[] FindKey(string pattern)
                {
                        return SyncRedisHelper.FindKey(this._DataBase, pattern);
                }

                public long GetCount(string key)
                {
                        return SyncRedisHelper.GetCount(this._DataBase, key);
                }

                public bool TryGetColName(string key, out string[] colname)
                {
                        return SyncRedisHelper.TryGetColName(this._DataBase, key, out colname);
                }

                public long Remove(string[] key)
                {
                        return SyncRedisHelper.Remove(this._DataBase, key);
                }

                public bool IsExitKey(string key, string colName)
                {
                        return SyncRedisHelper.IsExitKey(this._DataBase, key, colName);
                }

                public bool IsExitKey(string key)
                {
                        return SyncRedisHelper.IsExitKey(this._DataBase, key);
                }
                public bool SetExpire(string key, DateTime time)
                {
                        return SyncRedisHelper.SetExpire(this._DataBase, key, time);
                }

                public void Dispose()
                {
                }

                public T AddOrUpdate<T>(string key, string column, T data, Func<T, T, T> upFunc)
                {
                        if (SyncRedisHelper.TryGet<T>(this._DataBase, key, column, out T res))
                        {
                                data = upFunc(res, data);
                                if (data == null)
                                {
                                        return res;
                                }
                        }
                        return SyncRedisHelper.Set<T>(this._DataBase, key, column, data) ? data : default;
                }

                public T GetOrAdd<T>(string key, string column, T data)
                {
                        if (SyncRedisHelper.TryGet<T>(this._DataBase, key, column, out T obj))
                        {
                                return obj;
                        }
                        else
                        {
                                return SyncRedisHelper.Set<T>(this._DataBase, key, column, data)
                                        ? data
                                        : SyncRedisHelper.TryGet<T>(this._DataBase, key, column, out obj) ? obj : default;
                        }
                }

                public bool TryRemove<T>(string key, string column, out T data)
                {
                        if (!SyncRedisHelper.TryGet(this._DataBase, key, column, out data))
                        {
                                return false;
                        }
                        return SyncRedisHelper.Remove(this._DataBase, key, column);
                }

                public T TryUpdate<T>(string key, string column, T data, Func<T, T, T> upFunc)
                {
                        if (SyncRedisHelper.TryGet<T>(this._DataBase, key, column, out T res))
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
                        return SyncRedisHelper.Replace<T>(this._DataBase, key, column, data) ? data : res;
                }

                #region 递增和递减
                public double Increment(string key, double num)
                {

                        return SyncRedisHelper.Increment(this._DataBase, key, num);
                }
                public long Increment(string key, long num)
                {

                        return SyncRedisHelper.Increment(this._DataBase, key, num);
                }
                public double Decrement(string key, double num)
                {

                        return SyncRedisHelper.Decrement(this._DataBase, key, num);
                }
                public long Decrement(string key, long num)
                {

                        return SyncRedisHelper.Decrement(this._DataBase, key, num);
                }
                public double Increment(string key, string colname, double num)
                {

                        return SyncRedisHelper.Increment(this._DataBase, key, colname, num);
                }

                public long Increment(string key, string colname, long num)
                {

                        return SyncRedisHelper.Increment(this._DataBase, key, colname, num);
                }

                public double Decrement(string key, string colname, double num)
                {

                        return SyncRedisHelper.Decrement(this._DataBase, key, colname, num);
                }

                public long Decrement(string key, string colname, long num)
                {

                        return SyncRedisHelper.Decrement(this._DataBase, key, colname, num);
                }
                #endregion


                #region List
                public long ListInsert<T>(string key, T[] data)
                {

                        return SyncRedisHelper.ListAddLeft<T>(this._DataBase, key, data);
                }
                public long ListInsert<T>(string key, T data)
                {

                        return SyncRedisHelper.ListAddLeft<T>(this._DataBase, key, data);
                }
                public long ListRemove<T>(string key, T data)
                {

                        return SyncRedisHelper.ListRemove<T>(this._DataBase, key, data);
                }
                public bool GetList<T>(string key, int index, int size, out T[] data)
                {

                        return SyncRedisHelper.GetList<T>(this._DataBase, key, index, size, out data);
                }
                public long GetListCount(string key)
                {

                        return SyncRedisHelper.GetListCount(this._DataBase, key);
                }
                public int AddTopList<T>(string key, T data, int top)
                {

                        long num = SyncRedisHelper.ListAddLeft<T>(this._DataBase, key, data);
                        if (num > top)
                        {
                                SyncRedisHelper.ListTop(this._DataBase, key, top);
                                return top;
                        }
                        return (int)num;
                }
                public int AddTopList<T>(string key, T data, int start, int end)
                {

                        long num = SyncRedisHelper.ListAddLeft<T>(this._DataBase, key, data);
                        if (num > end)
                        {
                                SyncRedisHelper.ListTop(this._DataBase, key, start, end);
                                return end - start;
                        }
                        return (int)num;
                }

                public T TryUpdateList<T>(string key, int index, T data)
                {

                        if (!SyncRedisHelper.GetList<T>(this._DataBase, key, index, out T res))
                        {
                                return res;
                        }
                        SyncRedisHelper.SetList<T>(this._DataBase, key, index, data);
                        return data;
                }
                public long ListAppend<T>(string key, T[] data)
                {

                        return SyncRedisHelper.ListAddRight<T>(this._DataBase, key, data);
                }

                public long ListAppend<T>(string key, T data)
                {

                        return SyncRedisHelper.ListAddRight<T>(this._DataBase, key, data);
                }

                public bool GetList<T>(string key, int index, out T data)
                {

                        return SyncRedisHelper.GetList<T>(this._DataBase, key, index, out data);
                }
                public bool GetList<T>(string key, out T[] data)
                {

                        return SyncRedisHelper.GetList<T>(this._DataBase, key, out data);
                }


                #endregion
        }
}
