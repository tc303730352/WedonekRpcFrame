using System.Threading.Tasks;

using WeDonekRpc.CacheClient.Interface;

namespace WeDonekRpc.CacheClient.Redis
{
    /// <summary>
    /// 基于异步的先进后出集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncRedisStack<T> : AsyncBasicRedisQS<T>, IAsyncRedisStack<T>
    {
        public AsyncRedisStack (string name, bool isFull = false) : base(name, isFull)
        {

        }

        public Task<long> Push (T item)
        {
            return RedisHelper.LPushAsync(base.QueueName, item);
        }
        public Task<long> Push (T[] item)
        {
            return RedisHelper.LPushAsync(base.QueueName, item);
        }

    }
}
