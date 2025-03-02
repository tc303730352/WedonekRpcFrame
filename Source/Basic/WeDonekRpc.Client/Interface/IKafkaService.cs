using System;

using Confluent.Kafka;

using WeDonekRpc.Client.Kafka;
using WeDonekRpc.Client.Kafka.Interface;
using WeDonekRpc.Client.Queue.Model;

namespace WeDonekRpc.Client.Interface
{
    public interface IKafkaService
    {
        /// <summary>
        /// 是否已经初始化
        /// </summary>
        bool IsInit { get; }
        /// <summary>
        /// 生产消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="propertys"></param>
        /// <param name="topic"></param>
        /// <returns></returns>
        bool Producer (string msg, KafkaPropertys propertys, params string[] topic);
        bool Producer (string msg, params string[] topic);
        bool Producer<Key, T> (T msg, Key key, KafkaPropertys propertys, params string[] topic) where T : class;
        bool Producer<Key, T> (T msg, Key key, params string[] topic) where T : class;
        bool Producer<Key> (string msg, Key key, KafkaPropertys propertys, params string[] topic);
        bool Producer<Key> (string msg, Key key, params string[] topic);
        bool Producer<T> (T msg, KafkaPropertys propertys, params string[] topic) where T : class;
        bool Producer<T> (T msg, params string[] topic) where T : class;
        IkafkaTransaction ProducerTran (KafkaPropertys propertys);
        IkafkaTransaction<Key> ProducerTran<Key> (KafkaPropertys propertys);
        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="Key"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        IKafkaConsumer Subscrib<Key, T> (KafkaSubConfig config, Action<IKafkaEvent<Key, T>> action) where T : class;
        IKafkaConsumer Subscrib<Key, T> (Action<IKafkaEvent<Key, T>> action) where T : class;

        IKafkaConsumer Subscrib<T> (KafkaSubConfig config, Action<IKafkaEvent<Ignore, T>> action) where T : class;
        IKafkaConsumer Subscrib<T> (Action<IKafkaEvent<Ignore, T>> action) where T : class;
    }
}