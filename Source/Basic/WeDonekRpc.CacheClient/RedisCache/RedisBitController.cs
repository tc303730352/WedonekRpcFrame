using CSRedis;
using WeDonekRpc.CacheClient.Interface;

namespace WeDonekRpc.CacheClient.RedisCache
{
    internal class RedisBitController : RedisController, IRedisBitController
    {
        public bool SetBit (string key, uint office, bool bit)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.SetBit(key, office, bit);
        }
        public bool GetBit (string key, uint office)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.GetBit(key, office);
        }
        public long BitCount (string key, long start, long end)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.BitCount(key, start, end);
        }
        public long BitOp (string destKey, RedisBitOp op, params string[] keys)
        {
            destKey = RpcCacheService.FormatKey(destKey);
            keys = RpcCacheService.FormatKey(keys);
            return this._Client.BitOp(op, destKey, keys);
        }
        public long BitPos (string key, bool bit, long? start = null, long? end = null)
        {
            key = RpcCacheService.FormatKey(key);
            return this._Client.BitPos(key, bit, start, end);
        }
    }
}
