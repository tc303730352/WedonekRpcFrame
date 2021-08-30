using System;
using System.Threading.Tasks;

using RpcCacheClient.Cache;
using RpcCacheClient.Interface;

namespace RpcCacheClient.Redis
{
        public class AsyncRedisCacheController : AsyncCacheController, IAsyncRedisCache
        {

                public bool IsSupportBatch => this._Redis.IsSupportBatch;

                public bool IsSupportTran => this._Redis.IsSupportTran;

                private readonly IAsyncRedisCache _Redis = null;
                public AsyncRedisCacheController() : base(CacheType.Redis)
                {
                        this._Redis = (IAsyncRedisCache)this.CacheClient;
                }

                public async Task<bool> Add<T>(string key, string colname, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.Add<T>(key, colname, data);
                }


                public async Task<T> AddOrUpdate<T>(string key, string column, T data, Func<T, T, T> upFunc)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.AddOrUpdate<T>(key, column, data, upFunc);
                }


                public bool BeginBatch()
                {
                        return this._Redis.BeginBatch();
                }

                public bool BeginTran()
                {
                        return this._Redis.BeginTran();
                }

                public bool CommitTran()
                {
                        return this._Redis.CommitTran();
                }

                public async Task<double> Decrement(string key, double num)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.Decrement(key, num);
                }

                public async Task<long> Decrement(string key, long num)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.Decrement(key, num);
                }

                public async Task<double> Decrement(string key, string colname, double num)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.Decrement(key, colname, num);
                }

                public async Task<long> Decrement(string key, string colname, long num)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.Decrement(key, colname, num);
                }

                public void Dispose()
                {
                        this._Redis.Dispose();
                }

                public bool ExecBatch()
                {
                        return this._Redis.ExecBatch();
                }

                public async Task<string[]> FindKey(string pattern)
                {
                        pattern = RpcCacheService.FormatKey(pattern);
                        return await this._Redis.FindKey(pattern);
                }



                public async Task<T[]> GetAll<T>(string key)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.GetAll<T>(key);
                }

                public async Task<long> GetCount(string key)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.GetCount(key);
                }

                public async Task<T> GetOrAdd<T>(string key, string column, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.GetOrAdd(key, column, data);
                }



                public async Task<double> Increment(string key, double num)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.Increment(key, num);
                }

                public async Task<long> Increment(string key, long num)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.Increment(key, num);
                }

                public async Task<double> Increment(string key, string colname, double num)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.Increment(key, colname, num);
                }

                public async Task<long> Increment(string key, string colname, long num)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.Increment(key, colname, num);
                }

                public async Task<bool> IsExitKey(string key)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.IsExitKey(key);
                }

                public async Task<bool> IsExitKey(string key, string colName)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.IsExitKey(key, colName);
                }



                public async Task<bool> Remove(string key, string colname)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.Remove(key, colname);
                }

                public async Task<long> Remove(string key, string[] colname)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.Remove(key, colname);
                }

                public async Task<long> Remove(string[] key)
                {
                        Array.ForEach(key, a =>
                        {
                                a = RpcCacheService.FormatKey(a);
                        });
                        return await this._Redis.Remove(key);
                }

                public async Task<bool> Replace<T>(string key, string colname, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.Replace(key, colname, data);
                }



                public void ResetBatch()
                {
                        this._Redis.ResetBatch();
                }

                public async Task<bool> Set<T>(string key, string colname, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.Set(key, colname, data);
                }



                public async Task<T> Get<T>(string key, string colname)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.Get<T>(key, colname);
                }

                public async Task<string[]> GetListColName(string key)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.GetListColName(key);
                }


                public async Task<T> TryRemove<T>(string key, string column)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.TryRemove<T>(key, column);
                }

                public async Task<T> TryUpdate<T>(string key, string column, T data, Func<T, T, T> upFunc)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.TryUpdate<T>(key, column, data, upFunc);
                }

                public async Task<long> ListInsert<T>(string key, T[] data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.ListInsert<T>(key, data);
                }

                public async Task<long> ListInsert<T>(string key, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.ListInsert<T>(key, data);
                }

                public async Task<long> ListRemove<T>(string key, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.ListRemove<T>(key, data);
                }

                public async Task<T[]> GetList<T>(string key, int index, int size)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.GetList<T>(key, index, size);
                }

                public async Task<long> GetListCount<T>(string key)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.GetListCount<T>(key);
                }

                public async Task<int> AddTopList<T>(string key, T data, int top)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.AddTopList<T>(key, data, top);
                }

                public async Task<T> TryUpdateList<T>(string key, int index, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.TryUpdateList<T>(key, index, data);
                }

                public async Task<long> ListAppend<T>(string key, T[] data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.ListAppend<T>(key, data);
                }

                public async Task<long> ListAppend<T>(string key, T data)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.ListAppend<T>(key, data);
                }

                public async Task<T> GetList<T>(string key, int index)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.GetList<T>(key, index);
                }
                public async Task<int> AddTopList<T>(string key, T data, int start, int end)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.AddTopList<T>(key, data, start, end);
                }
                public async Task<T[]> GetList<T>(string key)
                {
                        key = RpcCacheService.FormatKey(key);
                        return await this._Redis.GetList<T>(key);
                }
        }
}
