using System;

using StackExchange.Redis;

namespace RpcCacheClient.Redis
{
        public class SyncBasicRedisQS<T>
        {
                protected readonly IDatabase _DataBase = null;
                private readonly string _QueueName = null;

                public string QueueName => this._QueueName;

                public SyncBasicRedisQS(string name, bool isFull = false)
                {
                        this._DataBase = RedisHelper.GetNewClient(name);
                        this._QueueName = isFull ? (this._QueueName = string.Join("_", name, Environment.MachineName)) : RpcCacheService.FormatKey(name);
                }

                public long GetCount()
                {
                        return SyncRedisHelper.GetListCount(this._DataBase, this._QueueName);
                }
                public bool CopyTo(T[] array, int index)
                {
                        if (SyncRedisHelper.GetList<T>(this._DataBase, this.QueueName, out T[] data) && data != null)
                        {
                                data.CopyTo(array, index);
                                return true;
                        }
                        return false;
                }
                public bool TryDequeue(out T data)
                {
                        return SyncRedisHelper.ListLeftPop<T>(this._DataBase, this._QueueName, out data);
                }

                public bool TryPeekRange(int index, int size, out T[] data)
                {
                        return SyncRedisHelper.GetList<T>(this._DataBase, this._QueueName, index, size, out data);
                }
                public bool TryPeek(out T data)
                {
                        return SyncRedisHelper.GetList<T>(this._DataBase, this._QueueName, 0, out data);
                }
                public bool TryPeek(out T[] data)
                {
                        return SyncRedisHelper.GetList<T>(this._DataBase, this.QueueName, out data);
                }
                public bool TryPeek(int index, out T data)
                {
                        return SyncRedisHelper.GetList<T>(this._DataBase, this.QueueName, index, out data);
                }
        }
}
