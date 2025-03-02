using System;
using System.Threading.Tasks;
using WeDonekRpc.Helper;

namespace WeDonekRpc.CacheClient.Redis
{
    public class AsyncBasicRedisQS<T>
    {
        private readonly string _QueueName = null;

        public string QueueName => this._QueueName;

        public AsyncBasicRedisQS (string name, bool isFull = false)
        {
            this._QueueName = isFull ? ( this._QueueName = string.Join("_", name, Environment.MachineName) ) : RpcCacheService.FormatKey(name);
        }

        public Task<T[]> ToArray ()
        {
            return RedisHelper.LRangeAsync<T>(this.QueueName, 0, -1);
        }

        public Task<long> GetCount ()
        {
            return RedisHelper.LLenAsync(this.QueueName);
        }
        public Task<bool> CopyTo (T[] array, int index)
        {
            return Task.Run<bool>(() =>
            {
                int len = array.Length - index;
                T[] res = RedisHelper.LRange<T>(this.QueueName, 0, len);
                if (res.IsNull())
                {
                    return false;
                }
                res.CopyTo(array, index);
                return true;
            });
        }
        public Task<T> TryDequeue ()
        {
            return RedisHelper.LPopAsync<T>(this.QueueName);
        }
        public Task<T[]> TryPeekRange (int index, int size)
        {
            return RedisHelper.LRangeAsync<T>(this._QueueName, index, size);
        }
        public Task<T> TryPeek ()
        {
            return RedisHelper.LIndexAsync<T>(this._QueueName, 0);
        }
        public Task<T> TryPeek (int index)
        {
            return RedisHelper.LIndexAsync<T>(this._QueueName, index);
        }

    }
}
