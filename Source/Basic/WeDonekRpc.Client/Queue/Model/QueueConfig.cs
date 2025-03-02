namespace WeDonekRpc.Client.Queue.Model
{
      
        public class QueueConfig
        {
                /// <summary>
                /// 集群Id
                /// </summary>
                public long RpcMerId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 队列类型
                /// </summary>
                public QueueType QueueType
                {
                        get;
                        set;
                } = QueueType.RabbitMQ;
                /// <summary>
                /// Rabbitmq配置
                /// </summary>
                public RabbitmqConfig Rabbitmq
                {
                        get;
                        set;
                }

                /// <summary>
                /// 系统类型
                /// </summary>
                public string SystemVal { get; set; }

                /// <summary>
                /// 系统组
                /// </summary>
                public string SysGroupVal { get; set; }
              

                /// <summary>
                /// 服务器ID
                /// </summary>
                public long ServerId
                {
                        get;
                        set;
                }
                /// <summary>
                /// 区域Id
                /// </summary>
                public int RegionId { get; set; }
                public KafkaConfig Kafka { get; internal set; }
        }
}
