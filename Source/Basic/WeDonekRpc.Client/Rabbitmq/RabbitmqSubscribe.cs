using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Log;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Rabbitmq
{
    [Attr.IgnoreIoc]
    internal class RabbitmqSubscribe : IQueueSubscribe
    {
        private IModel _Channel = null;
        private readonly RabbitmqSet _Config = null;
        private EventingBasicConsumer _Consumer = null;
        private readonly RabbitmqQueue _Rabbitmq;
        private readonly SubscribeEvent _Callback = null;

        public RabbitmqSubscribe ( RabbitmqQueue rabbitmq, RabbitmqSet config, SubscribeEvent action )
        {
            this._Config = config;
            this._Rabbitmq = rabbitmq;
            this._Callback = action;
            this._Init();
        }
        private void _Init ()
        {
            this._Channel = this._Rabbitmq.GetChannel();
            //消费者
            this._Consumer = new EventingBasicConsumer(this._Channel);
            if ( this._Config.IsAutoAck )
            {
                this._Consumer.Received += this._AutoAckReceived;
            }
            else
            {
                this._Consumer.Received += this._Received;
            }
            this._Consumer.Shutdown += this._Consumer_Shutdown;

        }

        private void _Consumer_Shutdown ( object sender, ShutdownEventArgs e )
        {
            new DebugLog("Rabbitmq订阅事件链接断开!", "Rpc_Rabbitmq")
                        {
                                {"ReplyCode",e.ReplyCode },
                                { "ReplyText",e.ReplyText},
                                { "ClassId",e.ClassId}
                        }.Save();
            if ( this._Channel.IsClosed )
            {
                this._Channel.Dispose();
                this._Init();
            }
        }
        private void _AutoAckReceived ( object sender, BasicDeliverEventArgs e )
        {
            byte[] bytes = e.Body.ToArray();
            string json = Encoding.UTF8.GetString(bytes);
            try
            {
                _ = this._Callback(json.Json<QueueRemoteMsg>(), e.RoutingKey, e.Exchange);
            }
            catch ( Exception ex )
            {
                ErrorException error = ErrorException.FormatError(ex);
                error.Args.Add("ConsumerTag", e.ConsumerTag);
                error.Args.Add("Exchange", e.Exchange);
                error.Args.Add("RoutingKey", e.RoutingKey);
                error.Args.Add("body", json);
                error.SaveLog("Rpc_Rabbitmq");
            }
        }
        private void _Received ( object sender, BasicDeliverEventArgs e )
        {
            byte[] bytes = e.Body.ToArray();
            string json = Encoding.UTF8.GetString(bytes);
            try
            {
                QueueRemoteMsg msg = json.Json<QueueRemoteMsg>();
                if ( msg == null || msg.Msg == null )
                {
                    this._Channel.BasicNack(e.DeliveryTag, false, false);
                }
                SubscribeErrorType res = this._Callback(msg, e.RoutingKey, e.Exchange);
                if ( res == SubscribeErrorType.Success )
                {
                    this._Channel.BasicAck(e.DeliveryTag, false);
                }
                else if ( res == SubscribeErrorType.Fail )
                {
                    this._Channel.BasicNack(e.DeliveryTag, false, false);
                }
                else
                {
                    this._Channel.BasicReject(e.DeliveryTag, false);
                }
            }
            catch ( Exception ex )
            {
                this._Channel.BasicNack(e.DeliveryTag, false, true);
                ErrorException error = ErrorException.FormatError(ex);
                error.Args.Add("ConsumerTag", e.ConsumerTag);
                error.Args.Add("Exchange", e.Exchange);
                error.Args.Add("RoutingKey", e.RoutingKey);
                error.Args.Add("body", json);
                error.SaveLog("Rpc_Rabbitmq");
            }
        }


        public void Dispose ()
        {
            this._Consumer?.OnCancel();
            this._Channel.Close();
            this._Channel.Dispose();
        }

        public void BindRoute ( string routeKey )
        {
            this._Config.Subscribs.ForEach(a =>
            {
                this._Channel.QueueBind(a.Queue, a.Exchange, routeKey);
            });
        }

        public void Subscrib ()
        {
            string[] queues = this._Config.Subscribs.Distinct(a => a.Queue);
            queues.ForEach(a =>
            {
                _ = this._Channel.BasicConsume(a, this._Config.IsAutoAck, this._Consumer);
            });
        }

    }
}
