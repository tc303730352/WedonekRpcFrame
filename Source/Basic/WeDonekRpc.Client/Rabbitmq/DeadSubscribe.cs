using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Rabbitmq.Interface;
using WeDonekRpc.Client.Rabbitmq.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Log;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Rabbitmq
{
    /// <summary>
    /// 订阅死信事件
    /// </summary>
    [Attr.IgnoreIoc]
    internal class DeadSubscribe : IDeadSubscribe
    {
        private IModel _Channel = null;
        private string[] _Queue = new string[0];
        private EventingBasicConsumer _Consumer = null;
        private readonly RabbitmqQueue _Rabbitmq;
        private readonly DeadMsg _Callback = null;
        private readonly string[] _DeadExchange = null;
        public DeadSubscribe ( RabbitmqQueue rabbitmq, MsgSource source, DeadMsg action )
        {
            this._DeadExchange = new string[]
            {
                string.Concat ("Rpc_Msg_Dead_", source.RegionId),
                string.Concat ("Rpc_Sub_Dead_", source.RegionId)
            };
            this._Rabbitmq = rabbitmq;
            this._Callback = action;
            this._Init();
        }
        private void _Init ()
        {
            this._Channel = this._Rabbitmq.CreateChannel();
            //消费者
            this._Consumer = new EventingBasicConsumer(this._Channel);
            this._Consumer.Received += this._Received;
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
                this._Init();
            }
        }

        private void _Received ( object sender, BasicDeliverEventArgs e )
        {
            byte[] bytes = e.Body.ToArray();
            string json = Encoding.UTF8.GetString(bytes);
            BroadcastType type = BroadcastType.消息队列;
            if ( e.Exchange.StartsWith("Rpc_Sub_Dead_") )
            {
                type = BroadcastType.订阅;
            }
            try
            {
                DeadQueueMsg msg = new DeadQueueMsg
                {
                    MsgBody = json,
                    Msg = json.Json<QueueRemoteMsg>()
                };
                this._Callback(msg, e.RoutingKey, type);
                this._Channel.BasicAck(e.DeliveryTag, false);
            }
            catch ( Exception ex )
            {
                this._Channel.BasicNack(e.DeliveryTag, false, false);
                new ErrorLog(ErrorException.FormatError(ex), "死信队列消息处理错误！", json, "Rpc_Rabbitmq")
                {
                        { "ConsumerTag",e.ConsumerTag},
                        { "Exchange",e.Exchange},
                        { "RoutingKey",e.RoutingKey}
                }.Save();
            }
        }


        public void Dispose ()
        {
            this._Consumer?.OnCancel();
            this._Channel.Close();
            this._Channel.Dispose();
        }

        public void BindQueue ( string queue )
        {
            if ( this._Queue.IsExists(queue) )
            {
                return;
            }
            this._Queue = this._Queue.Add(queue);
            _ = this._Channel.QueueDeclare(queue, true, false, false);
            this._DeadExchange.ForEach(a =>
           {
               this._Channel.QueueBind(queue, a, string.Empty);
           });
            _ = this._Channel.BasicConsume(queue, false, this._Consumer);
        }
    }
}
