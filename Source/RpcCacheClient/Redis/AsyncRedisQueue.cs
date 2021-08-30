using System.Threading.Tasks;

using RpcCacheClient.Interface;

namespace RpcCacheClient.Redis
{
        /// <summary>
        /// 基于异步的先进先出集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class AsyncRedisQueue<T> : AsyncBasicRedisQS<T>, IAsyncRedisQueue<T>
        {
                public AsyncRedisQueue(string name, bool isFull = false) : base(name, isFull)
                {

                }

                public async Task<long> Enqueue(T item)
                {
                        return await AsyncRedisHelper.ListAddRight<T>(this._DataBase, this.QueueName, item);
                }
                public async Task<long> Enqueue(T[] item)
                {
                        return await AsyncRedisHelper.ListAddRight<T>(this._DataBase, this.QueueName, item);
                }


        }
}
