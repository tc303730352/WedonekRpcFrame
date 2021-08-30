namespace RpcClient.Queue.Model
{
        public enum QueueType
        {
                RabbitMQ = 0
        }
        public class QueueConfig
        {
                public long RpcMerId
                {
                        get;
                        set;
                }
                public QueueType QueueType
                {
                        get;
                        set;
                } = QueueType.RabbitMQ;
                /// <summary>
                /// 用于连接的默认客户端提供的名称
                /// </summary>
                public string ClientProvidedName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 用户名
                /// </summary>
                public string UserName
                {
                        get;
                        set;
                }
                /// <summary>
                /// 密码
                /// </summary>
                public string Pwd { get; set; }

                /// <summary>
                /// 服务器链接
                /// </summary>
                public QueueCon[] Servers { get; set; }

                /// <summary>
                /// 虚拟地址
                /// </summary>
                public string VirtualHost { get; set; } = "/";
                /// <summary>
                /// 系统类型
                /// </summary>
                public string SystemVal { get; set; }

                /// <summary>
                /// 系统组
                /// </summary>
                public string SysGroupVal { get; set; }
                /// <summary>
                /// 过期时间(毫秒)
                /// </summary>
                public int Expiration { get; set; }
                /// <summary>
                /// 是否持久
                /// </summary>
                public bool IsLasting { get; set; } = false;

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
                /// <summary>
                /// 消息自动消费
                /// </summary>
                public bool IsAutoAck { get; set; } = true;
                /// <summary>
                /// 订阅消息是否自动消费
                /// </summary>
                public bool SubIsAutoAck { get; set; }
        }
}
