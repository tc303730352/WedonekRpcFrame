using System;
using System.Collections.Generic;

namespace WeDonekRpc.CacheClient.Interface
{
    public interface IRedisHashController
    {
        bool Add (string key, string field, object value);

        long Decrement (string key, string field);

        bool Exists (string key);

        bool Exists (string key, string fieId);

        Dictionary<string, string> Get (string key);

        string Get (string key, string field);

        Dictionary<string, T> Get<T> (string key);
        T Get<T> (string key, string field);
        string[] GetKeys (string key);
        string[] Gets (string key, params string[] field);
        T[] Gets<T> (string key, params string[] field);
        string[] GetValues (string key);
        T[] GetValues<T> (string key);
        decimal IncrBy (string key, string field, decimal num);
        long IncrBy (string key, string field, long num);
        long Increment (string key, string field);
        long Length (string key);
        bool Remove (string key);
        long Remove (string key, params string[] fieId);
        bool Set (string key, Dictionary<string, object> values);
        bool Set (string key, string field, object value);
        bool SetExpire (string key, TimeSpan time);
    }
}