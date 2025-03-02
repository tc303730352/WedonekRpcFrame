using System;
using System.Text;

using Confluent.Kafka;
using WeDonekRpc.Helper.Json;

namespace WeDonekRpc.Client.Kafka
{
    /// <summary>
    /// KafkaJson反序列化对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class KafkaJsonDeserializer<T> : IDeserializer<T> where T : class
    {
        public T Deserialize (ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (data.Length == 0)
            {
                return default;
            }
            string str = Encoding.UTF8.GetString(data.ToArray());
            return JsonTools.Json<T>(str);
        }
    }
}
