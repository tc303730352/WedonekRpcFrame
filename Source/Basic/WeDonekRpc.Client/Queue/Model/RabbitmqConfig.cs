using System;

namespace WeDonekRpc.Client.Queue.Model
{
    /// <summary>
    /// Rabbitmq配置
    /// </summary>
    public class RabbitmqConfig
    {
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
        /// 过期时间(毫秒)
        /// </summary>
        public int Expiration { get; set; } = 10000;

        public TimeSpan ConfirmTimeOut = new TimeSpan(0, 0, 5, 0);
        /// <summary>
        /// 是否持久
        /// </summary>
        public bool IsLasting { get; set; } = false;

        /// <summary>
        /// 消息自动消费
        /// </summary>
        public bool IsAutoAck { get; set; } = false;
        /// <summary>
        /// 订阅消息是否自动消费
        /// </summary>
        public bool SubIsAutoAck { get; set; } = false;
    }
}
