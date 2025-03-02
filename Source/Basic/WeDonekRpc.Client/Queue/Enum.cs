namespace WeDonekRpc.Client.Queue
{
        public enum QueueExchangeType
        {
                交换 = 0,
                广播 = 1,
                全局交换 = 2,
                全局广播 = 3
        }
        public enum QueueType
        {
                RabbitMQ = 0,
                Kafka = 1
        }
}
