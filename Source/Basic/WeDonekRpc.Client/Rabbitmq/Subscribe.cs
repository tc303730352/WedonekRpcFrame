using System;
using System.Collections.Generic;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using WeDonekRpc.Client.Rabbitmq.Interface;
using WeDonekRpc.Client.Rabbitmq.Model;

using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.Client.Rabbitmq
{
    [Attr.IgnoreIoc]
    internal class Subscribe : ISubscribe
    {
        private readonly IModel _Channel = null;
        private readonly EventingBasicConsumer _Consumer = null;
        private readonly List<string> _Queue = [];
        private readonly Action<ISubscribe, SubEventArgs> _Callback = null;
        private readonly bool _IsAutoAck = true;
        private readonly string _ExchangeType;
        public Subscribe ( RabbitmqQueue rabbitmq, string exchange, bool isAutoAck, string exchangeType, Action<ISubscribe, SubEventArgs> action )
        {
            this._ExchangeType = exchangeType;
            this._IsAutoAck = isAutoAck;
            this.Exchange = exchange;
            this._Channel = rabbitmq.CreateChannel();
            this._Callback = action;
            this._Consumer = new EventingBasicConsumer(this._Channel);
            this._Consumer.Received += this._Received;
        }
        public string Exchange
        {
            get;
        }
        public void Init ()
        {
            this._Channel.ExchangeDeclare(this.Exchange, this._ExchangeType, true, false, null);

        }

        private void _Received ( object sender, BasicDeliverEventArgs e )
        {
            SubEventArgs args = new SubEventArgs(e);
            try
            {
                this._Callback(this, args);
                if ( !this._IsAutoAck )
                {
                    this._Channel.BasicAck(e.DeliveryTag, false);
                }
            }
            catch ( Exception ex )
            {
                ErrorException error = ErrorException.FormatError(ex);
                if ( !this._IsAutoAck )
                {
                    this._Channel.BasicNack(e.DeliveryTag, false, error.IsSystemError);
                }
                new ErrorLog(error, "队列消息处理错误！", "Rpc_Rabbitmq")
                                {
                                        { "Msg",args.GetValue()},
                                        { "ConsumerTag",e.ConsumerTag},
                                        { "Exchange",e.Exchange},
                                        { "RoutingKey",e.RoutingKey}
                                }.Save();
            }
        }
        public void BindRoute ( string queue, string routeKey )
        {
            _ = this._Channel.QueueDeclare(queue, true, false, false, null);
            this._Channel.QueueBind(queue, this.Exchange, routeKey);
            this._Subscribe(queue);
        }
        public void BindRoute ( string queue, string[] routeKey )
        {
            _ = this._Channel.QueueDeclare(queue, true, false, false, null);
            routeKey.ForEach(a =>
             {
                 this._Channel.QueueBind(queue, this.Exchange, a);
             });
            this._Subscribe(queue);
        }
        public void QueueUnbind ( string queue, string routeKey )
        {
            this._Channel.QueueUnbind(queue, this.Exchange, routeKey);
            _ = this._Queue.Remove(queue);
        }

        private void _Subscribe ( string queue )
        {
            if ( !this._Queue.Contains(queue) )
            {
                _ = this._Channel.BasicConsume(queue, this._IsAutoAck, this._Consumer);
                this._Queue.Add(queue);
            }
        }

        public void Dispose ()
        {
            this._Consumer?.OnCancel();
            this._Channel.Close();
            this._Channel.Dispose();
        }
    }
}
