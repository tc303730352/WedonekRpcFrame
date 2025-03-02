using System;
using System.Collections.Concurrent;
using WeDonekRpc.CacheClient.Interface;
namespace WeDonekRpc.CacheClient.Redis
{
    public delegate void SubMsg<T> (T msg);
    /// <summary>
    /// 基于Redis 的订阅与关注
    /// </summary>
    internal class RedisSubPublic : IRedisSubPublic
    {
        private static readonly ConcurrentDictionary<string, ISubscriberController> _SubList = new ConcurrentDictionary<string, ISubscriberController>();

        /// <summary>
        /// 添加订阅
        /// </summary>
        /// <typeparam name="T">订阅传递的结构</typeparam>
        /// <param name="name">事件名</param>
        /// <param name="callback">触发的事件</param>
        /// <returns>是否成功</returns>
        public bool AddSubscriber<T> (string name, Action<T> callback)
        {
            string channel = RpcCacheService.FormatKey(name);
            if (!_SubList.TryGetValue(channel, out ISubscriberController subscriber))
            {
                subscriber = _SubList.GetOrAdd(channel, new SubscriberController<T>(channel, callback));
            }
            if (!subscriber.Init())
            {
                _ = _SubList.TryRemove(channel, out subscriber);
                subscriber.Dispose();
                return false;
            }
            return subscriber.IsInit;
        }

        public long PublicMsg<T> (string name, T data)
        {
            string channel = RpcCacheService.FormatKey(name);
            string msg = RedisTools.Serializable(data);
            return RedisHelper.Publish(channel, msg);
        }
        public void Remove (string name)
        {
            string channel = RpcCacheService.FormatKey(name);
            if (_SubList.TryRemove(channel, out ISubscriberController sub))
            {
                sub.Dispose();
            }
        }
    }
}
