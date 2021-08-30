using System;
using System.Threading.Tasks;

using RpcCacheClient.Interface;

using StackExchange.Redis;

namespace RpcCacheClient.Redis
{
        /// <summary>
        /// redis异步操作模型
        /// </summary>
        internal class AsyncRedisCache : IAsyncRedisCache
        {
                private readonly IDatabase _DataBase = null;
                public bool IsSupportTran
                {
                        get;
                }


                public CacheType CacheType
                {
                        get;
                }
                public bool IsSupportBatch
                {
                        get;
                }
                public AsyncRedisCache(int db = -1)
                {
                        this.IsSupportTran = true;
                        this.IsSupportBatch = true;
                        this._DataBase = AsyncRedisHelper.GetClient(db);
                }
                private volatile bool _IsBatch = false;
                private IBatch _Batch = null;
                public bool BeginBatch()
                {
                        if (!this._IsTran && !this._IsBatch)
                        {
                                this._Batch = this._DataBase.CreateBatch();
                                this._IsBatch = true;
                                return true;
                        }
                        return false;
                }
                public void ResetBatch()
                {
                        if (this._IsBatch)
                        {
                                this._Batch.Execute();
                                this._IsBatch = false;
                        }
                }
                public bool ExecBatch()
                {
                        if (this._IsBatch)
                        {
                                this._Batch.Execute();
                                return true;
                        }
                        return false;
                }

                private volatile bool _IsTran = false;
                private ITransaction _Tran = null;
                public bool BeginTran()
                {
                        if (!this._IsTran && !this._IsBatch)
                        {
                                this._Tran = this._DataBase.CreateTransaction();
                                this._IsTran = true;
                                return true;
                        }
                        return false;
                }
                public bool CommitTran()
                {
                        if (this._IsTran)
                        {
                                if (this._Tran.Execute())
                                {
                                        this._IsTran = false;
                                        return true;
                                }
                                return false;
                        }
                        return false;
                }
                private IDatabaseAsync _GetClinet()
                {
                        if (this._IsTran)
                        {
                                return this._Tran;
                        }
                        else if (this._IsBatch)
                        {
                                return this._Batch;
                        }
                        return this._DataBase;
                }
                public async Task<bool> Add<T>(string key, string colname, T data)
                {
                        return await AsyncRedisHelper.Add<T>(this._DataBase, key, colname, data);
                }

                public async Task<bool> Add<T>(string key, T data, TimeSpan expiresAt)
                {
                        return await AsyncRedisHelper.Add<T>(this._DataBase, key, data, expiresAt);
                }

                public async Task<bool> Add<T>(string key, T data)
                {
                        return await AsyncRedisHelper.Add<T>(this._DataBase, key, data);
                }

                public async Task<T> AddOrUpdate<T>(string key, T data, Func<T, T, T> upFunc)
                {
                        T res = await AsyncRedisHelper.Get<T>(this._DataBase, key);
                        if (res != null && !res.Equals(default(T)))
                        {
                                T up = upFunc(res, data);
                                if (up == null || up.Equals(res))
                                {
                                        return res;
                                }
                        }
                        return await AsyncRedisHelper.Set(this._DataBase, key, data) ? data : default;
                }

                public async Task<T> AddOrUpdate<T>(string key, T data, Func<T, T, T> upFunc, TimeSpan expiresAt)
                {
                        T res = await AsyncRedisHelper.Get<T>(this._DataBase, key);
                        if (res != null && !res.Equals(default(T)))
                        {
                                T up = upFunc(res, data);
                                if (up == null || up.Equals(res))
                                {
                                        return res;
                                }
                        }
                        return await AsyncRedisHelper.Set<T>(this._DataBase, key, data, expiresAt) ? data : default;
                }

                public async Task<string[]> FindKey(string pattern)
                {
                        return await AsyncRedisHelper.FindKey(this._DataBase, pattern);
                }

                public async Task<long> GetCount(string key)
                {
                        return await AsyncRedisHelper.GetCount(this._DataBase, key);
                }

                public async Task<T> GetOrAdd<T>(string key, T data)
                {
                        T res = await AsyncRedisHelper.Get<T>(this._DataBase, key);
                        if (res != null && !res.Equals(default(T)))
                        {
                                return res;
                        }
                        else if (await AsyncRedisHelper.Add<T>(this._DataBase, key, data))
                        {
                                return data;
                        }
                        else
                        {
                                return await AsyncRedisHelper.Get<T>(this._DataBase, key); ;
                        }
                }

                public async Task<T> GetOrAdd<T>(string key, T data, TimeSpan expiresAt)
                {
                        T res = await AsyncRedisHelper.Get<T>(this._DataBase, key);
                        if (res != null && !res.Equals(default(T)))
                        {
                                return res;
                        }
                        else if (await AsyncRedisHelper.Add<T>(this._DataBase, key, data, expiresAt))
                        {
                                return data;
                        }
                        else
                        {
                                return await AsyncRedisHelper.Get<T>(this._DataBase, key); ;
                        }
                }

                public async Task<bool> IsExitKey(string key, string colName)
                {
                        return await AsyncRedisHelper.IsExitKey(this._DataBase, key, colName);
                }

                public async Task<bool> IsExitKey(string key)
                {
                        return await AsyncRedisHelper.IsExitKey(this._DataBase, key);
                }

                public async Task<bool> Remove(string key, string colname)
                {
                        return await AsyncRedisHelper.Remove(this._DataBase, key, colname);
                }

                public async Task<long> Remove(string[] key)
                {
                        return await AsyncRedisHelper.Remove(this._DataBase, key);
                }

                public async Task<long> Remove(string key, string[] colname)
                {
                        return await AsyncRedisHelper.Remove(this._DataBase, key, colname);
                }

                public async Task<bool> Remove(string key)
                {
                        return await AsyncRedisHelper.Remove(this._DataBase, key);
                }

                public async Task<bool> Replace<T>(string key, string colname, T data)
                {
                        return await AsyncRedisHelper.Replace<T>(this._DataBase, key, colname, data);
                }

                public async Task<bool> Replace<T>(string key, T data, TimeSpan expiresAt)
                {
                        return await AsyncRedisHelper.Replace<T>(this._DataBase, key, data, expiresAt);
                }

                public async Task<bool> Replace<T>(string key, T data)
                {
                        return await AsyncRedisHelper.Replace<T>(this._DataBase, key, data);
                }

                public async Task<bool> Set<T>(string key, string colname, T data)
                {
                        return await AsyncRedisHelper.Set<T>(this._DataBase, key, colname, data);
                }

                public async Task<bool> Set<T>(string key, T data)
                {
                        return await AsyncRedisHelper.Set<T>(this._DataBase, key, data);
                }

                public async Task<bool> Set<T>(string key, T data, TimeSpan expiresAt)
                {
                        return await AsyncRedisHelper.Set<T>(this._DataBase, key, data, expiresAt);
                }

                public async Task<T> Get<T>(string key, string colname)
                {
                        return await AsyncRedisHelper.Get<T>(this._DataBase, key, colname);
                }

                public async Task<T[]> GetAll<T>(string key)
                {
                        return await AsyncRedisHelper.GetAll<T>(this._DataBase, key);
                }

                public async Task<T> Get<T>(string key)
                {
                        return await AsyncRedisHelper.Get<T>(this._DataBase, key);
                }

                public async Task<string[]> GetListColName(string key)
                {
                        return await AsyncRedisHelper.GetColName(this._DataBase, key);
                }

                public async Task<T> TryRemove<T>(string key)
                {
                        T res = await AsyncRedisHelper.Get<T>(this._DataBase, key);
                        return res == null || res.Equals(default(T)) ? res : await AsyncRedisHelper.Remove(this._DataBase, key) ? res : default;
                }

                public async Task<T> TryUpdate<T>(string key, T data, Func<T, T, T> upFunc)
                {
                        T res = await AsyncRedisHelper.Get<T>(this._DataBase, key);
                        if (res != null && !res.Equals(default(T)))
                        {
                                T up = upFunc(res, data);
                                if (up == null || up.Equals(res))
                                {
                                        return res;
                                }
                        }
                        else
                        {
                                return res;
                        }
                        return await AsyncRedisHelper.Replace<T>(this._DataBase, key, data) ? data : default;
                }

                public async Task<T> TryUpdate<T>(string key, T data, Func<T, T, T> upFunc, TimeSpan expiresAt)
                {
                        T res = await AsyncRedisHelper.Get<T>(this._DataBase, key);
                        if (res != null && !res.Equals(default(T)))
                        {
                                T up = upFunc(res, data);
                                if (up == null || up.Equals(res))
                                {
                                        return res;
                                }
                        }
                        else
                        {
                                return res;
                        }
                        return await AsyncRedisHelper.Replace(this._DataBase, key, data, expiresAt) ? data : default;
                }
                public async Task<T> AddOrUpdate<T>(string key, string column, T data, Func<T, T, T> upFunc)
                {
                        T res = await AsyncRedisHelper.Get<T>(this._DataBase, key);
                        if (res != null && !res.Equals(default(T)))
                        {
                                T up = upFunc(res, data);
                                if (up == null || up.Equals(res))
                                {
                                        return res;
                                }
                        }
                        return await AsyncRedisHelper.Set(this._DataBase, key, column, data) ? data : default;
                }
                public void Dispose()
                {
                        if (this._IsTran)
                        {
                                this.CommitTran();
                        }
                        else if (this._IsBatch)
                        {
                                this.ExecBatch();
                        }
                }

                public async Task<T> GetOrAdd<T>(string key, string column, T data)
                {
                        T res = await AsyncRedisHelper.Get<T>(this._DataBase, key);
                        if (res != null && !res.Equals(default(T)))
                        {
                                return res;
                        }
                        else
                        {
                                return await AsyncRedisHelper.Add<T>(this._DataBase, key, column, data) ? data : await AsyncRedisHelper.Get<T>(this._DataBase, key);
                        }
                }

                public async Task<T> TryRemove<T>(string key, string column)
                {
                        T res = await AsyncRedisHelper.Get<T>(this._DataBase, key, column);
                        return res == null || res.Equals(default(T)) ? res : await AsyncRedisHelper.Remove(this._DataBase, key, column) ? res : default;
                }

                public async Task<T> TryUpdate<T>(string key, string column, T data, Func<T, T, T> upFunc)
                {
                        T res = await AsyncRedisHelper.Get<T>(this._DataBase, key);
                        if (res != null && !res.Equals(default(T)))
                        {
                                T up = upFunc(res, data);
                                if (up == null || up.Equals(res))
                                {
                                        return res;
                                }
                        }
                        else
                        {
                                return default;
                        }
                        return await AsyncRedisHelper.Replace<T>(this._DataBase, key, column, data) ? data : res;
                }
                #region 递增和递减
                public async Task<double> Increment(string key, double num)
                {
                        return await AsyncRedisHelper.Increment(this._DataBase, key, num);
                }
                public async Task<long> Increment(string key, long num)
                {
                        return await AsyncRedisHelper.Increment(this._DataBase, key, num);
                }
                public async Task<double> Decrement(string key, double num)
                {
                        return await AsyncRedisHelper.Decrement(this._DataBase, key, num);
                }
                public async Task<long> Decrement(string key, long num)
                {
                        return await AsyncRedisHelper.Decrement(this._DataBase, key, num);
                }
                public async Task<double> Increment(string key, string colname, double num)
                {
                        return await AsyncRedisHelper.Increment(this._DataBase, key, colname, num);
                }

                public async Task<long> Increment(string key, string colname, long num)
                {
                        return await AsyncRedisHelper.Increment(this._DataBase, key, colname, num);
                }

                public async Task<double> Decrement(string key, string colname, double num)
                {
                        return await AsyncRedisHelper.Decrement(this._DataBase, key, colname, num);
                }

                public async Task<long> Decrement(string key, string colname, long num)
                {
                        return await AsyncRedisHelper.Decrement(this._DataBase, key, colname, num);
                }
                #endregion
                public async Task<T> GetList<T>(string key, int index)
                {
                        return await AsyncRedisHelper.GetList<T>(this._DataBase, key, index);
                }
                public async Task<T[]> GetList<T>(string key)
                {
                        return await AsyncRedisHelper.GetList<T>(this._DataBase, key);
                }
                public async Task<long> ListAppend<T>(string key, T data)
                {
                        return await AsyncRedisHelper.ListAddRight<T>(this._DataBase, key, data);
                }
                public async Task<long> ListAppend<T>(string key, T[] data)
                {
                        return await AsyncRedisHelper.ListAddRight<T>(this._DataBase, key, data);
                }
                public async Task<long> ListInsert<T>(string key, T data)
                {
                        return await AsyncRedisHelper.ListAddLeft<T>(this._DataBase, key, data);
                }
                public async Task<T> TryUpdateList<T>(string key, int index, T data)
                {
                        T res = await AsyncRedisHelper.GetList<T>(this._DataBase, key, index);
                        if (res == null || res.Equals(default(T)))
                        {
                                return res;
                        }
                        AsyncRedisHelper.SetList<T>(this._DataBase, key, index, data);
                        return data;
                }
                public async Task<long> ListInsert<T>(string key, T[] data)
                {
                        return await AsyncRedisHelper.ListAddLeft<T>(this._DataBase, key, data);
                }
                public async Task<int> AddTopList<T>(string key, T data, int top)
                {
                        long num = await AsyncRedisHelper.ListAddLeft<T>(this._DataBase, key, data);
                        if (num > top)
                        {
                                AsyncRedisHelper.ListTop(this._DataBase, key, top);
                                return top;
                        }
                        return (int)num;
                }
                public async Task<T[]> GetList<T>(string key, int index, int size)
                {
                        return await AsyncRedisHelper.GetList<T>(this._DataBase, key, index, size);
                }
                public async Task<long> GetListCount<T>(string key)
                {
                        return await AsyncRedisHelper.GetListCount(this._DataBase, key);
                }
                public async Task<long> ListRemove<T>(string key, T data)
                {
                        return await AsyncRedisHelper.ListRemove(this._DataBase, key, data);
                }
                public async Task<int> AddTopList<T>(string key, T data, int start, int end)
                {
                        long num = await AsyncRedisHelper.ListAddLeft<T>(this._DataBase, key, data);
                        if (num > end)
                        {
                                AsyncRedisHelper.ListTop(this._DataBase, key, start, end);
                                return end - start;
                        }
                        return (int)num;
                }
        }
}
