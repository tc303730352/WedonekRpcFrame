using System.Threading.Tasks;

using WeDonekRpc.CacheClient.Interface;

namespace WeDonekRpc.CacheClient.Redis
{
    /// <summary>
    /// 基于异步的先进先出集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncRedisQueue<T> : AsyncBasicRedisQS<T>, IAsyncRedisQueue<T>
    {
        public AsyncRedisQueue (string name, bool isFull = false) : base(name, isFull)
        {

        }

        public Task<long> Enqueue (T item)
        {
            return RedisHelper.RPushAsync(base.QueueName, item);
        }
        public Task<long> Enqueue (T[] item)
        {
            return RedisHelper.RPushAsync(base.QueueName, item);
        }


    }
}
