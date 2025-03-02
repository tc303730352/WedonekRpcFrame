using System;
using System.Threading.Tasks;

namespace WeDonekRpc.CacheClient.Interface
{
    public interface IAsyncRedisCache : IAsyncCacheController, IDisposable
    {
        Task<bool> Add<T> (string key, string colname, T data);
        Task<T> AddOrUpdate<T> (string key, string column, T data, Func<T, T, T> upFunc);

        bool BeginBatch ();
        bool BeginTran ();
        bool CommitTran ();
        Task<double> Decrement (string key, double num);
        Task<long> Decrement (string key, long num);
        Task<double> Decrement (string key, string colname, double num);
        Task<long> Decrement (string key, string colname, long num);
        bool ExecBatch ();
        Task<string[]> FindKey (string pattern);

        Task<T[]> GetAll<T> (string key);
        Task<long> GetCount (string key);
        Task<T> GetOrAdd<T> (string key, string column, T data);

        Task<double> Increment (string key, double num);
        Task<long> Increment (string key, long num);
        Task<double> Increment (string key, string colname, double num);
        Task<long> Increment (string key, string colname, long num);
        Task<bool> IsExitKey (string key);
        Task<bool> IsExitKey (string key, string colName);
        Task<bool> Remove (string key, string colname);
        Task<long> Remove (string key, string[] colname);
        Task<long> Remove (string[] key);
        Task<bool> Replace<T> (string key, string colname, T data);
        void ResetBatch ();
        Task<bool> Set<T> (string key, string colname, T data);
        Task<T> Get<T> (string key, string colname);
        Task<string[]> GetListColName (string key);
        Task<T> TryRemove<T> (string key, string column);
        Task<T> TryUpdate<T> (string key, string column, T data, Func<T, T, T> upFunc);

        Task<long> ListInsert<T> (string key, T[] data);
        Task<long> ListInsert<T> (string key, T data);
        Task<long> ListRemove<T> (string key, T data);
        Task<T[]> GetList<T> (string key, int index, int size);
        Task<long> GetListCount<T> (string key);
        Task<int> AddTopList<T> (string key, T data, int top);
        Task<int> AddTopList<T> (string key, T data, int start, int end);

        Task<T> TryUpdateList<T> (string key, int index, T data);
        Task<long> ListAppend<T> (string key, T[] data);

        Task<long> ListAppend<T> (string key, T data);

        Task<T> GetList<T> (string key, int index);
        Task<T[]> GetList<T> (string key);
    }
}