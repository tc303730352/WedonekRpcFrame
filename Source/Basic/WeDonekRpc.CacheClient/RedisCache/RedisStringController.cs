using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Helper;

namespace WeDonekRpc.CacheClient.RedisCache
{
    internal class RedisStringController : RedisController, IRedisStringController
    {
        public decimal IncrBy (string key, decimal num)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.IncrByFloat(key, num);
        }
        public long IncrBy (string key, long num)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.IncrBy(key, num);
        }
        public long Decrement (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.IncrBy(key, -1);
        }
        public long Increment (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.IncrBy(key, 1);
        }
        public long Length (string key)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.StrLen(key);
        }
        public string Substring (string key, int start, int end)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.GetRange(key, start, end);
        }
        public string GetSet (string key, string value)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.GetSet(key, value);
        }
        public T GetSet<T> (string key, T value)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.GetSet<T>(key, value);
        }
        public T[] Gets<T> (params string[] keys)
        {
            keys = RpcCacheService.FormatKey(keys);
            return this._Client.MGet<T>(keys);
        }
        public string[] Gets (params string[] keys)
        {
            keys = RpcCacheService.FormatKey(keys);
            return this._Client.MGet(keys);
        }

        public long SetRange (string key, uint office, string value)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.SetRange(key, office, value);
        }
        public bool Set (string[] keys, string[] vals)
        {
            if (keys.Length != vals.Length)
            {
                throw new ErrorException("cache.redis.key.value.not.equal");
            }
            object[] datas = new object[keys.Length + vals.Length];
            int index = 0;
            for (int i = 0; i < datas.Length; i += 2)
            {
                datas[i] = RpcCacheService.FormatKey(keys[index]);
                datas[i + 1] = vals[index];
                index += 1;
            }
            return this._Client.MSet(datas);
        }
        public long Append (string key, string value)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.Append(key, value);
        }
        public bool Add (string[] keys, string[] vals)
        {
            if (keys.Length != vals.Length)
            {
                throw new ErrorException("cache.redis.key.value.not.equal");
            }
            object[] datas = new object[keys.Length + vals.Length];
            int index = 0;
            for (int i = 0; i < datas.Length; i += 2)
            {
                datas[i] = RpcCacheService.FormatKey(keys[index]);
                datas[i + 1] = vals[index];
                index += 1;
            }
            return this._Client.MSetNx(datas);
        }
    }
}
