using System;

using Confluent.Kafka;

using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Kafka.Interface;
using WeDonekRpc.Client.Queue.Model;

using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.Client.Kafka
{
    [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.SingleInstance)]
    internal class KafkaService : IKafkaService
    {
        private readonly KafkaQueue _Kafka;

        public bool IsInit { get; }
        public KafkaService ()
        {
            KafkaConfig config = Config.WebConfig.GetKafkaConfig();
            if ( config != null )
            {
                this._Kafka = new KafkaQueue(config);
                this.IsInit = true;
            }
        }
        #region 发布
        public bool Producer ( string msg, params string[] topic )
        {
            return this._Producer<Null>(msg, null, null, topic);
        }
        public bool Producer<Key> ( string msg, Key key, KafkaPropertys propertys, params string[] topic )
        {
            return this._Producer(msg, key, propertys, topic);
        }
        public bool Producer<Key> ( string msg, Key key, params string[] topic )
        {
            return this._Producer(msg, key, null, topic);
        }
        public bool Producer ( string msg, KafkaPropertys propertys, params string[] topic )
        {
            return this._Producer<Null>(msg, null, propertys, topic);
        }

        public bool Producer<T> ( T msg, params string[] topic ) where T : class
        {
            return this._Producer<Null>(msg.ToJson(), null, null, topic);
        }
        public bool Producer<Key, T> ( T msg, Key key, KafkaPropertys propertys, params string[] topic ) where T : class
        {
            return this._Producer(msg.ToJson(), key, propertys, topic);
        }
        public bool Producer<Key, T> ( T msg, Key key, params string[] topic ) where T : class
        {
            return this._Producer(msg.ToJson(), key, null, topic);
        }
        public bool Producer<T> ( T msg, KafkaPropertys propertys, params string[] topic ) where T : class
        {
            return this._Producer<Null>(msg.ToJson(), null, propertys, topic);
        }
        private bool _Producer<Key> ( string msg, Key key, KafkaPropertys propertys, string[] topic )
        {
            Message<Key, string> message = new Message<Key, string>
            {
                Value = msg,
                Key = key,
                Timestamp = new Timestamp(DateTime.Now),
                Headers = []
            };
            propertys?.InitHeader(message.Headers);
            ProducerConfig config = this._Kafka.GetPublicConfig(propertys);
            using ( IProducer<Key, string> p = new ProducerBuilder<Key, string>(config).Build() )
            {
                try
                {
                    topic.ForEach(a =>
                    {
                        p.Produce(a, message);
                    });
                    p.Flush();
                    return true;
                }
                catch ( ProduceException<Key, byte[]> e )
                {
                    new ErrorLog(e.GetBaseException(), "Kafka发布错误", "Kafka")
                                        {
                                                {"ErrorCode",e.Error.Code },
                                                {"Reason",e.Error.Reason },
                                                { "Msg",msg}
                                        }.Save();
                    return false;
                }
            }
        }
        #endregion


        public IkafkaTransaction ProducerTran ( KafkaPropertys propertys )
        {
            kafkaTran tran = new kafkaTran(this._Kafka, propertys);
            tran.Begin();
            return tran;
        }
        public IkafkaTransaction<Key> ProducerTran<Key> ( KafkaPropertys propertys )
        {
            kafkaTran<Key> tran = new kafkaTran<Key>(this._Kafka, propertys);
            tran.Begin();
            return tran;
        }

        public IKafkaConsumer Subscrib<Key, T> ( KafkaSubConfig config, Action<IKafkaEvent<Key, T>> action ) where T : class
        {
            return new KafkaConsumer<Key, T>(this._Kafka, config, action);
        }
        public IKafkaConsumer Subscrib<Key, T> ( Action<IKafkaEvent<Key, T>> action ) where T : class
        {
            return new KafkaConsumer<Key, T>(this._Kafka, null, action);
        }

        public IKafkaConsumer Subscrib<T> ( KafkaSubConfig config, Action<IKafkaEvent<Ignore, T>> action ) where T : class
        {
            return new KafkaConsumer<Ignore, T>(this._Kafka, config, action);
        }

        public IKafkaConsumer Subscrib<T> ( Action<IKafkaEvent<Ignore, T>> action ) where T : class
        {
            return new KafkaConsumer<Ignore, T>(this._Kafka, null, action);
        }
    }
}
