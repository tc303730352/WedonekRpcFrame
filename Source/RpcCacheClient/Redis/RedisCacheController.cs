using System;

using RpcCacheClient.Cache;
using RpcCacheClient.Interface;
namespace RpcCacheClient.Redis
{
        /// <summary>
        /// redis缓存控制器
        /// </summary>
        public class RedisCacheController : CacheController, IRedisCacheController
        {
                private readonly IRedisCacheController _Redis = null;
                /// <summary>
                /// 是否支持事务
                /// </summary>
                public bool IsSupportTran => this._Redis.IsSupportTran;
                /// <summary>
                /// 是否支持批量
                /// </summary>
                public bool IsSupportBatch => this._Redis.IsSupportBatch;
                public RedisCacheController(int db = 0) : base(CacheType.Redis, db)
                {
                        this._Redis = (IRedisCacheController)this.CacheClient;
                }
                public bool Add<T>(string key, string colname, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.Add<T>(key, colname, data);
                }
                public bool Set<T>(string key, string colname, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.Add<T>(key, colname, data);
                }
                public bool Replace<T>(string key, string colname, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.Add<T>(key, colname, data);
                }

                public bool Remove(string key, string colname)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.Remove(key, colname);
                }

                public bool TryGet<T>(string key, string colname, out T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.TryGet(key, colname, out data);
                }

                public bool TryGet<T>(string key, out T[] data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.TryGet(key, out data);
                }

                public long Remove(string[] key)
                {
                        Array.ForEach(key, a =>
                        {
                                a = RpcCacheService.FormatKey(a);
                        });
                        return this._Redis.Remove(key);
                }

                public long Remove(string key, string[] colname)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.Remove(key, colname);
                }

                public string[] FindKey(string pattern)
                {
                        pattern = RpcCacheService.FormatKey(pattern);
                        return this._Redis.FindKey(pattern);
                }

                public long GetCount(string key)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.GetCount(key);
                }
                public T AddOrUpdate<T>(string key, string column, T data, Func<T, T, T> upFunc)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.AddOrUpdate(key, column, data, upFunc);
                }

                public bool TryGetColName(string key, out string[] colname)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.TryGetColName(key, out colname);
                }

                public bool IsExitKey(string key, string colName)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.IsExitKey(key, colName);
                }

                public bool IsExitKey(string key)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.IsExitKey(key);
                }
                public bool SetExpire(string key, DateTime time)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.SetExpire(key, time);
                }
                public void Dispose()
                {
                        this._Redis.Dispose();
                }

                public T GetOrAdd<T>(string key, string column, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.GetOrAdd<T>(key, column, data);
                }

                public bool TryRemove<T>(string key, string column, out T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.TryRemove<T>(key, column, out data);
                }

                public T TryUpdate<T>(string key, string column, T data, Func<T, T, T> upFunc)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.TryUpdate<T>(key, column, data, upFunc);
                }
                #region 递增和递减
                public double Increment(string key, double num)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.Increment(key, num);
                }
                public long Increment(string key, long num)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.Increment(key, num);
                }
                public double Decrement(string key, double num)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.Decrement(key, num);
                }
                public long Decrement(string key, long num)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.Decrement(key, num);
                }
                public double Increment(string key, string colname, double num)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.Increment(key, colname, num);
                }

                public long Increment(string key, string colname, long num)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.Increment(key, colname, num);
                }

                public double Decrement(string key, string colname, double num)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.Decrement(key, colname, num);
                }

                public long Decrement(string key, string colname, long num)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.Decrement(key, colname, num);
                }
                #endregion

                #region List
                public long ListInsert<T>(string key, T[] data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.ListInsert<T>(key, data);
                }
                public long ListInsert<T>(string key, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.ListInsert<T>(key, data);
                }
                public long ListRemove<T>(string key, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.ListRemove<T>(key, data);
                }
                public bool GetList<T>(string key, int index, int size, out T[] data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.GetList<T>(key, index, size, out data);
                }
                public long GetListCount(string key)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.GetListCount(key);
                }
                public int AddTopList<T>(string key, T data, int top)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.AddTopList<T>(key, data, top);
                }
                public int AddTopList<T>(string key, T data, int start, int end)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.AddTopList<T>(key, data, start, end);
                }

                public T TryUpdateList<T>(string key, int index, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.TryUpdateList<T>(key, index, data);
                }
                public long ListAppend<T>(string key, T[] data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.ListAppend<T>(key, data);
                }

                public long ListAppend<T>(string key, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.ListAppend<T>(key, data);
                }

                public bool GetList<T>(string key, int index, out T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.GetList<T>(key, index, out data);
                }
                public bool GetList<T>(string key, out T[] data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return this._Redis.GetList<T>(key, out data);
                }
                #endregion
        }
}
