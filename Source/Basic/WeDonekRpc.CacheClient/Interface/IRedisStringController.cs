namespace WeDonekRpc.CacheClient.Interface
{
    public interface IRedisStringController : IRedisController
    {
        bool Add (string[] keys, string[] vals);

        long Append (string key, string value);

        long Decrement (string key);

        string[] Gets (params string[] keys);

        T[] Gets<T> (params string[] keys);

        string GetSet (string key, string value);

        T GetSet<T> (string key, T value);

        long Increment (string key);

        long Length (string key);

        bool Set (string[] keys, string[] vals);

        long SetRange (string key, uint office, string value);

        string Substring (string key, int start, int end);

        long IncrBy (string key, long num);
        decimal IncrBy (string key, decimal num);
    }
}