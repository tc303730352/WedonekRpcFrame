using System;

namespace WeDonekRpc.CacheClient.Interface
{
    public interface IRedisListController
    {
        bool Remove (string key);
        long Append<T> (string key, params T[] value);

        long AppendExist<T> (string key, T value);

        long Count (string key);

        bool Exists (string key);

        T Get<T> (string key, int index);

        T[] Gets<T> (string key);

        T[] Gets<T> (string key, int start);

        T[] Gets<T> (string key, int start, int end);

        long Insert<T> (string key, params T[] value);

        long InsertAfter<T> (string key, int index, T value);

        long InsertBefore<T> (string key, int index, T value);

        long InsertExist<T> (string key, T value);

        T LeftPop<T> (string key);

        (string key, T value)? LeftPopWait<T> (int timeOut, params string[] keys);

        T LeftPopWait<T> (string key, int timeOut);

        T LeftPopWait<T> (string key, string dest, int timeOut);

        long Remove<T> (string key, T data, int count = 0);
        T RightPop<T> (string key);
        (string key, T value)? RightPopWait<T> (int timeOut, params string[] keys);
        T RightPopWait<T> (string key, int timeOut);
        T RightPopWait<T> (string key, string dest, int timeOut);
        bool SetExpire (string key, TimeSpan time);
        bool Trim (string key, int start, int end);
    }
}