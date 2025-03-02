
using Confluent.Kafka;

using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Queue.Model;

using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Kafka
{
    internal class KafkaQueue
    {
        private readonly ClientConfig _BasicConfig = null;

        private readonly KafkaConfig _Config;
        public KafkaQueue (KafkaConfig config)
        {
            this._Config = config;
            ClientConfig client = config.ConvertMap<KafkaConfig, ClientConfig>();
            client.ClientId = string.Concat("Server_", RpcStateCollect.ServerId);
            if (this._Config.Consumer.GroupId.IsNull())
            {
                this._Config.Consumer.GroupId = "RpcService";
            }
            client.ClientRack = RpcStateCollect.LocalSource.SystemType;
            client.BootstrapServers = config.Servers.Join(",", a => a.ToString(9092));
            this._BasicConfig = client;
        }
        public ConsumerConfig GetConsumerConfig (KafkaSubConfig config)
        {
            ConsumerConfig con = new ConsumerConfig(this._BasicConfig);
            if (config == null)
            {
                return con.ConvertInto(this._Config.Consumer);
            }
            return con.ConvertInto(config);
        }
        public ProducerConfig GetPublicConfig (KafkaPropertys pro)
        {
            ProducerConfig config = new ProducerConfig(this._BasicConfig);
            config = config.ConvertInto(this._Config.Public);
            if (pro == null)
            {
                return config;
            }
            else if (!pro.TranId.IsNull())
            {
                config.Acks = Acks.All;
                config.TransactionalId = pro.TranId;
                if (pro.TransactionTimeoutMs.HasValue)
                {
                    config.TransactionTimeoutMs = pro.TransactionTimeoutMs.Value;
                }
            }
            else if (pro.Ask.HasValue)
            {
                config.Acks = pro.Ask.Value;
            }
            return config;
        }
    }
}
