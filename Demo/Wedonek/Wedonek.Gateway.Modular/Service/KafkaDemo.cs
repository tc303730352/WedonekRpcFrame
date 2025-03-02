using System;

using Confluent.Kafka;

using Wedonek.Gateway.Modular.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Kafka.Interface;
using WeDonekRpc.Helper;

namespace Wedonek.Gateway.Modular.Service
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class KafkaDemo : IKafkaDemo
    {
        private readonly IKafkaService _Kafka = null;
        private readonly IKafkaConsumer _Consumer;
        private readonly string[] _Topic = new string[] { "Demo_Queue" };
        public KafkaDemo (IKafkaService kafka)
        {
            this._Kafka = kafka;
            if (this._Kafka.IsInit)
            {
                this._Consumer = this._Kafka.Subscrib<string>(this._Subscrib);
            }
        }

        public void Dispose ()
        {
            this._Consumer?.Dispose();
        }

        public void Producer ()
        {
            if (!this._Kafka.Producer("Demo_Msg", this._Topic))
            {
                throw new ErrorException("demo.kafka.send.fail");
            }
        }
        public void Subscribe ()
        {
            this._Consumer.AddBind(this._Topic);
            this._Consumer.Subscribe();
        }

        private void _Subscrib (IKafkaEvent<Ignore, string> obj)
        {
            Console.WriteLine("Kafka_Topic：" + obj.Topic);
            Console.WriteLine("收到Kafka订阅消息：" + obj.Value);
        }
    }
}
