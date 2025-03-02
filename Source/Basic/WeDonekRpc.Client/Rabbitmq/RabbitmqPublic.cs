using System.Text;
using RabbitMQ.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Rabbitmq
{
    [Attr.IgnoreIoc]
    internal class RabbitmqPublic : IQueuePublic
    {
        private readonly RabbitmqQueue _Rabbitmq = null;
        public RabbitmqPublic (RabbitmqQueue quque)
        {
            this._Rabbitmq = quque;
        }

        public bool Public (string[] routeKey, string exchange, QueueRemoteMsg msg)
        {
            using (IModel channel = this._Rabbitmq.CreateChannel())
            {
                IBasicProperties props = this._Rabbitmq.GetProperties(channel);
                byte[] datas = Encoding.UTF8.GetBytes(msg.ToJson());
                routeKey.ForEach(a =>
                {
                    channel.BasicPublish(exchange, a, false, props, datas);
                });
                return true;
            }
        }

        public bool PublicTran (string[] routeKey, string exchange, QueueRemoteMsg msg)
        {
            using (IModel channel = this._Rabbitmq.CreateChannel())
            {
                IBasicProperties props = this._Rabbitmq.GetProperties(channel);
                byte[] datas = Encoding.UTF8.GetBytes(msg.ToJson());
                channel.ConfirmSelect();
                routeKey.ForEach(a =>
                {
                    channel.BasicPublish(exchange, a, false, props, datas);
                });
                channel.WaitForConfirmsOrDie(this._Rabbitmq.Config.ConfirmTimeOut);
                return true;
            }
        }
    }
}
