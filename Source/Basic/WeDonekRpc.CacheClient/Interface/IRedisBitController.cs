using CSRedis;

namespace WeDonekRpc.CacheClient.Interface
{
    public interface IRedisBitController : IRedisController
    {
        long BitCount (string key, long start, long end);
        long BitOp (string destKey, RedisBitOp op, params string[] keys);
        long BitPos (string key, bool bit, long? start = null, long? end = null);
        bool GetBit (string key, uint office);
        bool SetBit (string key, uint office, bool bit);
    }
}