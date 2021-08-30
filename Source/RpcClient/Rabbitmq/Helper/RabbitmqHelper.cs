using RabbitMQ.Client;

using RpcClient.Queue.Model;

using RpcHelper;

namespace RpcClient.Rabbitmq.Helper
{
        internal class RabbitmqHelper : System.IDisposable
        {
                private readonly IModel _Channel = null;
                public RabbitmqHelper(IModel channel)
                {
                        this._Channel = channel;
                }

                public void BindExchange(string[] exchanges)
                {
                        exchanges.ForEach(a =>
                        {
                                this._Channel.ExchangeDeclare(a, "direct", true, false, null);
                        });
                }

                public void Dispose()
                {
                        this._Channel.Close();
                        this._Channel.Dispose();
                }

                public void QueueBind(string[] exchanges, QueueSubInfo[] subs)
                {
                        subs.ForEach(a =>
                        {
                                exchanges.ForEach(b =>
                                {
                                        this._Channel.QueueBind(a.Queue, b, a.RouteKey);
                                });
                        });
                }

                internal void QueueDeclare(string[] queues)
                {
                        queues.ForEach(a =>
                        {
                                this._Channel.QueueDeclare(a, true, false, false);
                        });
                }
        }
}
