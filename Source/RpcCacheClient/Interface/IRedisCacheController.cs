using System;

namespace RpcCacheClient.Interface
{
        public interface IRedisCacheController : ICacheController, System.IDisposable
        {
                bool IsSupportBatch { get; }
                bool IsSupportTran { get; }

                bool IsExitKey(string key, string colName);
                bool IsExitKey(string key);
                bool SetExpire(string key, DateTime time);
                bool Add<T>(string key, string colname, T data);
                bool Set<T>(string key, string colname, T data);
                bool Replace<T>(string key, string colname, T data);

                bool Remove(string key, string colname);
                long Remove(string[] key);
                bool TryGet<T>(string key, string colname, out T data);
                bool TryGet<T>(string key, out T[] data);
                long Remove(string key, string[] colname);

                string[] FindKey(string pattern);

                long GetCount(string key);

                bool TryGetColName(string key, out string[] colname);

                T AddOrUpdate<T>(string key, string column, T data, Func<T, T, T> upFunc);

                T GetOrAdd<T>(string key, string column, T data);

                bool TryRemove<T>(string key, string column, out T data);

                T TryUpdate<T>(string key, string column, T data, Func<T, T, T> upFunc);

                double Increment(string key, double num);
                long Increment(string key, long num);
                double Decrement(string key, double num);
                long Decrement(string key, long num);

                double Increment(string key, string colname, double num);
                long Increment(string key, string colname, long num);
                double Decrement(string key, string colname, double num);
                long Decrement(string key, string colname, long num);

                #region List
                long ListInsert<T>(string key, T[] data);
                long ListInsert<T>(string key, T data);
                long ListRemove<T>(string key, T data);
                bool GetList<T>(string key, int index, int size, out T[] data);
                long GetListCount(string key);
                int AddTopList<T>(string key, T data, int top);
                int AddTopList<T>(string key, T data, int start, int end);

                T TryUpdateList<T>(string key, int index, T data);
                long ListAppend<T>(string key, T[] data);

                long ListAppend<T>(string key, T data);

                bool GetList<T>(string key, int index, out T data);
                bool GetList<T>(string key, out T[] data);
                #endregion
        }
}