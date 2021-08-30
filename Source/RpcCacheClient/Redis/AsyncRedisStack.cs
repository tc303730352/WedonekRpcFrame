using System.Threading.Tasks;

using RpcCacheClient.Interface;

namespace RpcCacheClient.Redis
{
        /// <summary>
        /// 基于异步的先进后出集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class AsyncRedisStack<T> : AsyncBasicRedisQS<T>, IAsyncRedisStack<T>
        {
                public AsyncRedisStack(string name, bool isFull = false) : base(name, isFull)
                {

                }

                public async Task<long> Push(T item)
                {
                        return await AsyncRedisHelper.ListAddLeft<T>(this._DataBase, this.QueueName, item);
                }
                public async Task<long> Push(T[] item)
                {
                        return await AsyncRedisHelper.ListAddLeft<T>(this._DataBase, this.QueueName, item);
                }

        }
}
