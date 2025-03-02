using System;
using CSRedis;
using WeDonekRpc.Helper;

namespace WeDonekRpc.CacheClient.Redis
{
    public class SyncBasicRedisQS<T>
    {
        private readonly string _QueueName = null;

        public string QueueName => this._QueueName;

        private readonly bool _IsMuilt = true;
        public SyncBasicRedisQS (string name, bool isLocal = false)
        {
            this._QueueName = isLocal ? ( this._QueueName = string.Join("_", name, Environment.MachineName) ) : RpcCacheService.FormatKey(name);
        }

        public long GetCount ()
        {
            return RedisHelper.LLen(this._QueueName);
        }
        public bool CopyTo (T[] array, int index)
        {
            int len = array.Length - index;
            T[] res = RedisHelper.LRange<T>(this.QueueName, 0, len);
            if (res.IsNull())
            {
                return false;
            }
            res.CopyTo(array, index);
            return true;
        }
        public bool TryDequeue (out T data)
        {
            string res = RedisHelper.LPop(this._QueueName);
            if (res == null)
            {
                data = default;
                return false;
            }
            object obj = StringParseTools.Parse(res, typeof(T));
            data = (T)obj;
            return true;
        }
        public T[] Dequeue (int count)
        {
            if (this.GetCount() == 0)
            {
                return null;
            }
            CSRedisClientPipe<string> pipe = RedisHelper.StartPipe();
            _ = pipe.LRange(this.QueueName, 0, count);
            _ = pipe.LTrim(this.QueueName, count, -1);
            object[] res = pipe.EndPipe();
            Type type = typeof(T);
            if (type == PublicDataDic.StrType)
            {
                return (T[])res[0];
            }
            return ( (string[])res[0] ).ConvertAll<string, T>(c => StringParseTools.Parse(c, type));
        }

        public bool TryPeekRange (int index, int size, out T[] data)
        {
            data = RedisHelper.LRange<T>(this._QueueName, index, size);
            return !data.IsNull();
        }
        public bool TryPeek (out T data)
        {
            string res = RedisHelper.LIndex(this._QueueName, 0);
            if (res != null)
            {
                data = StringParseTools.Parse(res, typeof(T));
                return true;
            }
            data = default;
            return false;
        }
        public bool TryPeek (out T[] data)
        {
            data = RedisHelper.LRange<T>(this._QueueName, 0, -1);
            return !data.IsNull();
        }
        public bool TryPeek (int index, out T data)
        {
            string res = RedisHelper.LIndex(this._QueueName, index);
            if (res != null)
            {
                data = StringParseTools.Parse(res, typeof(T));
                return true;
            }
            data = default;
            return false;
        }

    }
}
