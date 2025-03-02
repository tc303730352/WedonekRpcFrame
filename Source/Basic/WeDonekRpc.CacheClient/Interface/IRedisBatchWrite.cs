using System;

namespace WeDonekRpc.CacheClient.Interface
{
    public interface IRedisBatchWrite
    {
        void Add<T>(string key, string colname, T data);
        void Add<T>(string key, T data, TimeSpan? expires = null);
        void Decrement(string key, double num);
        void Decrement(string key, long num);
        void Decrement(string key, string colname, double num);
        void Decrement(string key, string colname, long num);
        void Increment(string key, double num);
        void Increment(string key, long num);
        void Increment(string key, string colname, double num);
        void Increment(string key, string colname, long num);
        void ListAppend<T>(string key, T data);
        void ListAppend<T>(string key, T[] data);
        void ListInsert<T>(string key, T data);
        void ListInsert<T>(string key, T[] data);
        void ListRemove<T>(string key, T data);
        void ListTop(string key, int start, int end);
        void Remove(string key);
        void Remove(string key, string colname);
        void Remove(string key, string[] colname);
        void Remove(string[] key);
        void Replace<T>(string key, string colname, T data);
        void Replace<T>(string key, T data, TimeSpan? expires = null);
        void Set<T>(string key, string colname, T data);
        void Set<T>(string key, T data, TimeSpan? expires = null);
        void Set<T>(string key, T data, DateTime expires);
        void SetExpire(string key, DateTime time);
        object[] Submit();
    }
}