using System;
using System.Collections.Generic;
using System.Text;

using Confluent.Kafka;

using WeDonekRpc.Client.Kafka.Interface;

using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Kafka
{
        internal class kafkaTran<Key> : IkafkaTransaction<Key>
        {
                private bool _IsComplate = false;
                private readonly KafkaQueue _Kafka;
                private readonly KafkaPropertys _Propertys;
                private IProducer<Key, string> _Produce;
                public kafkaTran(KafkaQueue queue, KafkaPropertys propertys)
                {
                        this._Propertys = propertys;
                        if (propertys.TranId.IsNull())
                        {
                                propertys.TranId = Guid.NewGuid().ToString("N");
                        }
                        this._Kafka = queue;
                }
                public void Begin()
                {
                        ProducerConfig config = this._Kafka.GetPublicConfig(this._Propertys);
                        this._Produce = new ProducerBuilder<Key, string>(config).Build();
                        this._Produce.InitTransactions(new TimeSpan(0, 0, this._Propertys.TranInitTimeOut));
                        this._Produce.BeginTransaction();
                }
                public void Commit()
                {
                        if (!this._IsComplate)
                        {
                                this._IsComplate = true;
                                this._Produce.CommitTransaction();
                        }
                }

                public void Dispose()
                {
                        if (!this._IsComplate)
                        {
                                this._IsComplate = true;
                                this._Produce.AbortTransaction();
                        }
                        this._Produce.Dispose();
                }

                public bool Producer(string msg, Key key, params string[] topic)
                {
                        return this._Send(msg, key, topic);
                }

                public bool Producer<T>(T msg, Key key, params string[] topic) where T : class
                {
                        return this._Send(msg.ToJson(), key, topic);
                }



                private bool _Send(string msg, Key key, string[] topic)
                {
                        Message<Key, string> message = new Message<Key, string>
                        {
                                Value = msg,
                                Key = key,
                                Timestamp = new Timestamp(DateTime.Now),
                                Headers = new Headers()
                        };
                        this._Propertys.InitHeader(message.Headers);
                        try
                        {
                                topic.ForEach(a =>
                                {
                                        this._Produce.Produce(a, message);
                                });
                                return true;
                        }
                        catch (ProduceException<Key, byte[]> e)
                        {
                                return false;
                        }
                }
        }
}
