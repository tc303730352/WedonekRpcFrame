using System;
using System.Collections.Generic;
using RabbitMQ.Client;

using WeDonekRpc.Client.Queue.Model;

using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Rabbitmq.Helper
{
    internal class RabbitmqHelper : System.IDisposable
    {
        private readonly IModel _Channel = null;
        private readonly RabbitmqConfig _Config = null;
        public RabbitmqHelper (RabbitmqQueue queue)
        {
            this._Config = queue.Config;
            this._Channel = queue.CreateChannel();
        }

        public void BindExchange (string[] exchanges, string type = "direct")
        {
            exchanges.ForEach(a =>
             {
                 this._Channel.ExchangeDeclare(a, type, true, false, null);
             });
        }

        public void Dispose ()
        {
            this._Channel.Close();
            this._Channel.Dispose();
        }

        public void QueueBind (string[] exchanges, QueueSubInfo[] subs)
        {
            subs.ForEach(a =>
             {
                 exchanges.ForEach(b =>
                             {
                                 this._Channel.QueueBind(a.Queue, b, a.RouteKey);
                             });
             });
        }

        internal void QueueDeclare (string[] queues, Dictionary<string, object> args)
        {
            queues.ForEach(a =>
             {
                 try
                 {
                     _ = this._Channel.QueueDeclare(a, true, false, false, args);
                 }
                 catch (Exception)
                 {

                 }
             });
        }
    }
}
