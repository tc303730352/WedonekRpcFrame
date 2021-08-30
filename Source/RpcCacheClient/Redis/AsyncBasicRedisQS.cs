using System;
using System.Threading.Tasks;

using StackExchange.Redis;

namespace RpcCacheClient.Redis
{
        public class AsyncBasicRedisQS<T>
        {
                protected readonly IDatabaseAsync _DataBase = null;
                private readonly string _QueueName = null;

                public string QueueName => this._QueueName;


                public AsyncBasicRedisQS(string name, bool isFull = false)
                {
                        this._DataBase = RedisHelper.GetNewClient(name);
                        this._QueueName = isFull ? (this._QueueName = string.Join("_", name, Environment.MachineName)) : RpcCacheService.FormatKey(name);
                }
                public async Task<T[]> ToArray()
                {
                        return await AsyncRedisHelper.GetList<T>(this._DataBase, this.QueueName);
                }

                public async Task<long> GetCount()
                {
                        return await AsyncRedisHelper.GetListCount(this._DataBase, this._QueueName);
                }
                public async Task<bool> CopyTo(T[] array, int index)
                {
                        T[] res = await AsyncRedisHelper.GetList<T>(this._DataBase, this._QueueName);
                        if (res != null && res.Length != 0)
                        {
                                res.CopyTo(array, index);
                                return true;
                        }
                        return false;
                }
                public async Task<T> TryDequeue()
                {
                        return await AsyncRedisHelper.ListLeftPop<T>(this._DataBase, this._QueueName);
                }
                public async Task<T[]> TryPeekRange(int index, int size)
                {
                        return await AsyncRedisHelper.GetList<T>(this._DataBase, this._QueueName, index, size);
                }
                public async Task<T> TryPeek()
                {
                        return await AsyncRedisHelper.GetList<T>(this._DataBase, this._QueueName, 0);
                }
                public async Task<T> TryPeek(int index)
                {
                        return await AsyncRedisHelper.GetList<T>(this._DataBase, this.QueueName, index);
                }
        }
}
