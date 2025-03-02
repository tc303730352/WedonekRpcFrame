using WeDonekRpc.CacheClient.Interface;

namespace WeDonekRpc.CacheClient.Redis
{
    /// <summary>
    /// 先进后出的集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SyncRedisStack<T> : SyncBasicRedisQS<T>, ISyncRedisStack<T>
    {
        public SyncRedisStack (string name, bool isFull = false) : base(name, isFull)
        {

        }
        public long Push (T item)
        {
            return RedisHelper.LPush<T>(this.QueueName, item);
        }
        public long Push (T[] item)
        {
            return RedisHelper.LPush<T>(this.QueueName, item);
        }
    }
}
