
using Confluent.Kafka;

namespace WeDonekRpc.Client.Queue.Model
{
        public class KafkaPublicConfig
        {
                public Acks? Acks
                {
                        get;
                        set;
                } = Confluent.Kafka.Acks.All;
                public int? BatchNumMessages { get; set; } = 10000;
                /// <summary>
                /// 压缩类型
                /// </summary>
                public CompressionType? CompressionType { get; set; } = Confluent.Kafka.CompressionType.None;

                public int? QueueBufferingBackpressureThreshold { get; set; } = 1;

                /// <summary>
                /// 消息发送失败重试时间间隔
                /// </summary>
                public int? RetryBackoffMs { get; set; } = 100;

                public int? MessageSendMaxRetries { get; set; } = 2147483647;

                /// <summary>
                /// 指定ProducerBatch在延迟多少毫秒后再发送
                /// </summary>
                public double? LingerMs { get; set; } = 5f;

                public int? QueueBufferingMaxKbytes { get; set; } = 1048576;

                public int? QueueBufferingMaxMessages { get; set; } = 100000;

                public bool? EnableGaplessGuarantee { get; set; } = false;


                /// <summary>
                /// 事务超时时间
                /// </summary>
                public int? TransactionTimeoutMs { get; set; } = 60000;
                /// <summary>
                /// 指定ProducerBatch内存区域的大小
                /// </summary>
                public int? BatchSize { get; set; } = 1048576;

                /// <summary>
                /// 压缩级别
                /// </summary>
                public int? CompressionLevel { get; set; } = -1;

                public Partitioner? Partitioner { get; set; } = Confluent.Kafka.Partitioner.ConsistentRandom;

                /// <summary>
                /// 消息超时时间
                /// </summary>
                public int? MessageTimeoutMs { get; set; } = 300000;
                /// <summary>
                /// 请求超时时间
                /// </summary>
                public int? RequestTimeoutMs { get; set; } = 30000;

                public string DeliveryReportFields { get; set; } = "all";

                public bool? EnableDeliveryReports { get; set; } = true;

                public bool? EnableBackgroundPoll { get; set; } = true;

                public bool? EnableIdempotence { get; set; } = false;

                public int? StickyPartitioningLingerMs { get; set; } = 10;

                public int TranInitTimeOut { get; set; } = 30;
        }
}
