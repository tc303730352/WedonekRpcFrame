using System;

namespace WeDonekRpc.Client.Kafka.Interface
{
    /// <summary>
    /// Kafka消费者
    /// </summary>
    public interface IKafkaConsumer : IDisposable
    {
        /// <summary>
        /// 关闭事件
        /// </summary>
        event Action<string> CloseEvent;
        /// <summary>
        /// 添加绑定的队列
        /// </summary>
        /// <param name="queue">队列</param>
        void AddBind(string[] queue);
        /// <summary>
        /// 订阅
        /// </summary>
        void Subscribe();
    }
}