
using System;
using Confluent.Kafka;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Controller;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Kafka.Interface;
using WeDonekRpc.Client.Queue.Model;
using WeDonekRpc.Client.Rabbitmq.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.Model.Kafka.Model;

namespace WeDonekRpc.Client.Kafka
{
    internal class QueueKafka : IQueue
    {
        private readonly IKafkaService _Service;

        private readonly SubscribBody[] _SubBodys;
        private readonly IKafkaConsumer _Consumer;
        private readonly SubscribeEvent _SubEvent;
        private readonly KafkaConfig _Config;
        public QueueKafka (SubscribBody[] bodys, KafkaConfig config, SubscribeEvent action)
        {
            this._Config = config;
            this._SubBodys = bodys;
            this._SubEvent = action;
            this._Service = new KafkaService();
            this._Consumer = this._Service.Subscrib<QueueRemoteMsg>(this._QueueMsg);
        }

        private void _QueueMsg (IKafkaEvent<Ignore, QueueRemoteMsg> obj)
        {
            DateTime expiration = obj.GetHeader<DateTime>("Expiration");
            if (expiration != DateTime.MinValue && expiration <= DateTime.Now)
            {
                return;
            }
            _ = this._SubEvent(obj.Value, obj.Topic, obj.GetHeader("Exchange"));
        }

        public void BindRoute (string routeKey)
        {
            ExchangeDatum[] exchanges = this._SubBodys.ConvertAll(a =>
            {
                return new ExchangeDatum
                {
                    Exchange = a.Exchange,
                    RouteKey = new ExchangeRouteKey[] {
                        new ExchangeRouteKey
                        {
                                Queue = a.Queue,
                                RouteKey = routeKey
                        }
                     }
                };
            });
            KafkaCollect.AddExchange(exchanges);
        }


        public void Dispose ()
        {
            this._Consumer.Dispose();
        }


        public bool Public (QueueRemoteMsg msg, string[] routeKey, string exchange)
        {
            if (!KafkaCollect.GetExchange(exchange, out KafkaExchangeController obj))
            {
                return false;
            }
            string[] queues = obj.GetQueue(routeKey);
            if (queues.Length == 0)
            {
                return false;
            }
            KafkaPropertys pro = new KafkaPropertys
            {
                TranInitTimeOut = this._Config.Public.TranInitTimeOut
            };
            pro.SetHeader("Exchange", exchange);
            if (this._Config.Expiration != 0)
            {
                pro.SetHeader("Exchange", DateTime.Now.AddSeconds(this._Config.Expiration));
            }
            if (!msg.TranId.HasValue)
            {
                return this._Public(pro, msg, queues);
            }
            return this._PublicTran(pro, msg, queues);
        }
        private bool _Public (KafkaPropertys pro, QueueRemoteMsg msg, string[] queues)
        {
            return this._Service.Producer(msg, pro, queues);
        }
        private bool _PublicTran (KafkaPropertys pro, QueueRemoteMsg msg, string[] queues)
        {
            pro.TranId = msg.TranId.Value.ToString();
            using (IkafkaTransaction tran = this._Service.ProducerTran(pro))
            {
                if (tran.Producer(msg, queues))
                {
                    tran.Commit();
                    return true;
                }
                return false;
            }
        }
        public void Subscrib ()
        {
            if (this._SubBodys.Length > 0)
            {
                string[] queues = this._SubBodys.ConvertAll(a => a.Queue);
                this._Consumer.AddBind(queues.Distinct());
                this._Consumer.Subscribe();
            }
        }
    }
}
