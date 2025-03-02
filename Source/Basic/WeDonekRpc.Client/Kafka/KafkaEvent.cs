using System.Text;

using Confluent.Kafka;

using WeDonekRpc.Client.Helper;
using WeDonekRpc.Client.Kafka.Interface;

namespace WeDonekRpc.Client.Kafka
{
    internal class KafkaEvent<Key, T> : IKafkaEvent<Key, T>
    {
        private readonly Message<Key, T> _Msg;

        public KafkaEvent(Message<Key, T> result, string quque)
        {
            this._Msg = result;
            this.Topic = quque;
        }
        public string Topic { get; }


        public Key MsgKey => this._Msg.Key;

        public T Value => this._Msg.Value;
        public string GetHeader(string name)
        {
            if (!_Msg.Headers.TryGetLastBytes(name, out byte[] data))
            {
                return null;
            }
            return Encoding.UTF8.GetString(data);
        }

        public Value GetHeader<Value>(string name)
        {
            if (!_Msg.Headers.TryGetLastBytes(name, out byte[] data))
            {
                return default;
            }
            return (Value)QueueHelper.GetValue(data, typeof(Value));
        }
    }
}
