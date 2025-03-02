
using Confluent.Kafka;

using WeDonekRpc.Client.Kafka.Interface;

namespace WeDonekRpc.Client.Kafka
{
        internal class kafkaTran : kafkaTran<Null>, IkafkaTransaction
        {
                public kafkaTran(KafkaQueue queue, KafkaPropertys propertys):base(queue, propertys)
                {
                      
                }
               

                public bool Producer(string msg, params string[] topic)
                {
                       return base.Producer(msg, null, topic);
                }

                public bool Producer<T>(T msg, params string[] topic) where T : class
                {
                       return base.Producer(msg, null, topic);
                }
              
        }
}
