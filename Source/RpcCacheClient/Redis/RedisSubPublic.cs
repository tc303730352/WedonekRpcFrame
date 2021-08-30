using System;
using System.Collections.Concurrent;

using RpcCacheClient.Interface;

using StackExchange.Redis;
namespace RpcCacheClient.Redis
{
        public delegate void SubMsg<T>(T msg);
        /// <summary>
        /// 基于Redis 的订阅与关注
        /// </summary>
        public class RedisSubPublic
        {
                private static readonly ConcurrentDictionary<string, ISubscriberController> _SubList = new ConcurrentDictionary<string, ISubscriberController>();

                /// <summary>
                /// 添加订阅
                /// </summary>
                /// <typeparam name="T">订阅传递的结构</typeparam>
                /// <param name="name">事件名</param>
                /// <param name="callback">触发的事件</param>
                /// <returns>是否成功</returns>
                public static bool AddSubscriber<T>(string name, Action<T> callback)
                {
                        Type type = typeof(T);
                        string channel = string.Join("_", name, typeof(T).FullName);
                        if (!_SubList.TryGetValue(channel, out ISubscriberController subscriber))
                        {
                                subscriber = _SubList.GetOrAdd(channel, new SubscriberController<T>(channel, callback));
                        }
                        if (!subscriber.Init())
                        {
                                _SubList.TryRemove(channel, out subscriber);
                                subscriber.Dispose();
                                return false;
                        }
                        return subscriber.IsInit;
                }

                public static long PublicMsg<T>(string name, T data)
                {
                        ISubscriber client = RedisHelper.GetPublic();
                        string channel = string.Join("_", name, typeof(T).FullName);
                        RedisValue val = RedisTools.Serializable<T>(data);
                        return client.Publish(channel, val);
                }
                public static void Remove(string name, Type type)
                {
                        string channel = string.Join("_", name, type.FullName);
                        if (_SubList.TryRemove(channel, out ISubscriberController sub))
                        {
                                sub.Dispose();
                        }
                }
        }
}
