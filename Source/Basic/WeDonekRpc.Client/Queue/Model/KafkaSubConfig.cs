
using Confluent.Kafka;

namespace WeDonekRpc.Client.Queue.Model
{
        public class KafkaSubConfig
        {
                /// <summary>
                /// 启用EOF结束位
                /// </summary>
                public bool EnablePartitionEof { get; set; } = false;

                /// <summary>
                /// 事务隔离级别
                /// </summary>
                public IsolationLevel IsolationLevel { get; set; } = IsolationLevel.ReadCommitted;
                /// <summary>
                /// 出现错误时重试延迟
                /// </summary>
                public int FetchErrorBackoffMs { get; set; } = 200;
                /// <summary>
                /// 代理响应的最小字节数
                /// </summary>
                public int FetchMinBytes { get; set; } = 1;

                public int FetchMaxBytes { get; set; } = 52428800;

                public int MaxPartitionFetchBytes { get; set; } = 1048576;

                public int FetchWaitMaxMs { get; set; } = 500;

                public int QueuedMaxMessagesKbytes { get; set; } = 65536;

                public int QueuedMinMessages { get; set; } = 100000;

                public bool EnableAutoOffsetStore { get; set; } = true;

                public int AutoCommitIntervalMs { get; set; } = 1000;

                public bool EnableAutoCommit { get; set; } = true;

                public int MaxPollIntervalMs { get; set; } = 300000;

                public int CoordinatorQueryIntervalMs { get; set; } = 600000;


                public int HeartbeatIntervalMs { get; set; } = 3000;

                public int SessionTimeoutMs { get; set; } = 45000;
                public string GroupProtocolType { get; set; } = "consumer";
                public PartitionAssignmentStrategy PartitionAssignmentStrategy { get; set; } = Confluent.Kafka.PartitionAssignmentStrategy.Range;

                public string GroupInstanceId { get; set; } = string.Empty;

                public string GroupId { get; set; } = string.Empty;

                public AutoOffsetReset AutoOffsetReset { get; set; } = AutoOffsetReset.Latest;

                public string ConsumeResultFields { set; get; }

                public bool CheckCrcs { get; set; } = false;

                public bool AllowAutoCreateTopics { get; set; } = false;
        }
}
