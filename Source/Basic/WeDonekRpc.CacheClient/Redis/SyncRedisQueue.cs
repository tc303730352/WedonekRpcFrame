using WeDonekRpc.CacheClient.Interface;

namespace WeDonekRpc.CacheClient.Redis
{
    /// <summary>
    /// 先进先出的集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SyncRedisQueue<T> : SyncBasicRedisQS<T>, ISyncRedisQueue<T>
    {
        public SyncRedisQueue (string name, bool isFull = false) : base(name, isFull)
        {

        }

        public long Enqueue (T item)
        {
            return RedisHelper.RPush<T>(this.QueueName, item);
        }
        public long Enqueue (T[] item)
        {
            return RedisHelper.RPush<T>(this.QueueName, item);
        }


    }
}
