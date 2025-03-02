using System;
using CSRedis;

namespace WeDonekRpc.CacheClient.Interface
{
    public interface IRedisSetController
    {
        long Add<T> (string key, params T[] val);
        string[] Differenced (params string[] keys);
        long Differenced (string destination, params string[] keys);
        T[] Differenced<T> (params string[] keys);
        bool Exists (string key);
        bool Exists<T> (string key, T value);
        string GetRandom (string key);
        T GetRandom<T> (string key);
        string[] GetRandoms (string key);
        T[] GetRandoms<T> (string key);
        string[] Gets (string key);
        T[] Gets<T> (string key);
        RedisScan<T> Gets<T> (string key, long index, string pattern = null, long? count = null);
        long Intersects (string destination, string[] keys);
        string[] Intersects (string[] keys);
        long Length (string key);
        bool Move<T> (string key, string destination, object val);
        string Pop (string key);
        string[] Pop (string key, long count);
        T Pop<T> (string key);
        T[] Pop<T> (string key, long count);
        long Remove<T> (string key, params T[] vals);
        bool SetExpire (string key, TimeSpan time);
        string[] Union (string[] key);
        long Union<T> (string destination, params string[] key);
        T[] Union<T> (string[] key);
    }
}