using System;
using System.Text;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using RpcClient.Collect;
using RpcClient.Interface;

using RpcModel;

using RpcHelper;

namespace RpcClient.Rabbitmq
{
        [Attr.IgnoreIoc]
        internal class RabbitmqSubscribe : IQueueSubscribe
        {
                private readonly IModel _Channel = null;
                private readonly RabbitmqConfig _Config = null;
                private EventingBasicConsumer _Consumer = null;

                private readonly SubscribeEvent _Callback = null;

                public RabbitmqSubscribe(RabbitmqQueue rabbitmq, RabbitmqConfig config, SubscribeEvent action)
                {
                        this._Config = config;
                        this._Channel = rabbitmq.CreateChannel();
                        this._Callback = action;
                        this._Init();
                }
                private void _Init()
                {
                        //消费者
                        this._Consumer = new EventingBasicConsumer(this._Channel);
                        this._Consumer.Received += this._Received;
                }

                private void _Received(object sender, BasicDeliverEventArgs e)
                {
                        byte[] bytes = e.Body.ToArray();
                        string json = Encoding.UTF8.GetString(bytes);
                        try
                        {
                                if (this._Callback(json.Json<QueueRemoteMsg>(), e.RoutingKey, e.Exchange))
                                {
                                        this._Channel.BasicAck(e.DeliveryTag, false);
                                }
                                else
                                {
                                        this._Channel.BasicNack(e.DeliveryTag, false, false);
                                }
                        }
                        catch (Exception ex)
                        {
                                this._Channel.BasicNack(e.DeliveryTag, false, true);
                                new ErrorLog(ErrorException.FormatError(ex), "队列消息处理错误！", json, "Rabbitmq")
                                {
                                        { "ConsumerTag",e.ConsumerTag},
                                        { "Exchange",e.Exchange},
                                        { "RoutingKey",e.RoutingKey}
                                }.Save();
                        }
                }


                public void Dispose()
                {
                        if (this._Consumer != null)
                        {
                                this._Consumer.OnCancel();
                        }
                        this._Channel.Close();
                        this._Channel.Dispose();
                }

                public void BindRoute(string routeKey)
                {
                        this._Config.Subscribs.ForEach(a =>
                        {
                                this._Channel.QueueBind(a.Queue, a.Exchange, routeKey);
                        });
                }

                public void Subscrib()
                {
                        string[] queues = this._Config.Subscribs.Distinct(a => a.Queue);
                        queues.ForEach(a =>
                        {
                                this._Channel.BasicConsume(a, this._Config.IsAutoAck, this._Consumer);
                        });
                }

        }
}
