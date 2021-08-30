using System;
using System.Text;

using RabbitMQ.Client;

using RpcClient.Interface;

using RpcModel;

using RpcHelper;

namespace RpcClient.Rabbitmq
{
        [Attr.IgnoreIoc]
        internal class RabbitmqPublic : IQueuePublic
        {
                private readonly RabbitmqQueue _Rabbitmq = null;
                private readonly bool _IsLasting = false;
                private readonly string _Expiration = null;
                public RabbitmqPublic(RabbitmqQueue quque)
                {
                        this._IsLasting = quque.Config.IsLasting;
                        this._Expiration = quque.Config.Expiration.ToString();
                        this._Rabbitmq = quque;
                }
                public bool Public(string[] routeKey, string exchange, QueueRemoteMsg msg)
                {
                        if (msg.Msg.Tran != null)
                        {
                                return this._PublicTran(routeKey, exchange, msg);
                        }
                        return this._Public(routeKey, exchange, msg);
                }
                private bool _Public(string[] routeKey, string exchange, QueueRemoteMsg msg)
                {
                        using (IModel channel = this._Rabbitmq.CreateChannel())
                        {
                                IBasicProperties props = this._Create(channel);
                                byte[] datas = Encoding.UTF8.GetBytes(msg.ToJson());
                                routeKey.ForEach(a =>
                                {
                                        channel.BasicPublish(exchange, a, false, props, datas);
                                });
                                return true;
                        }
                }
                private IBasicProperties _Create(IModel channel)
                {
                        IBasicProperties props = channel.CreateBasicProperties();
                        props.ContentType = "application/json";
                        if (this._IsLasting)
                        {
                                props.DeliveryMode = 2;
                        }
                        else
                        {
                                props.DeliveryMode = 1;
                        }
                        props.Timestamp = new AmqpTimestamp(DateTime.Now.ToLong());
                        props.Expiration = this._Expiration.ToString();
                        return props;
                }
                private bool _PublicTran(string[] routeKey, string exchange, QueueRemoteMsg msg)
                {
                        using (IModel channel = this._Rabbitmq.CreateChannel())
                        {
                                IBasicProperties props = this._Create(channel);
                                byte[] datas = Encoding.UTF8.GetBytes(msg.ToJson());
                                channel.ConfirmSelect();
                                routeKey.ForEach(a =>
                                {
                                        channel.BasicPublish(exchange, a, false, props, datas);
                                });
                                try
                                {
                                        channel.WaitForConfirmsOrDie();
                                }
                                catch (Exception)
                                {
                                        return false;
                                }
                                return true;
                        }
                }
        }
}
