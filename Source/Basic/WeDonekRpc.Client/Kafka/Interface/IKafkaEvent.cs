namespace WeDonekRpc.Client.Kafka.Interface
{
    /// <summary>
    /// Kafka事件器
    /// </summary>
    /// <typeparam name="Key">消息Key类型</typeparam>
    /// <typeparam name="T">消息体</typeparam>
    public interface IKafkaEvent<Key, T>
    {
        /// <summary>
        /// 获取消息头
        /// </summary>
        /// <typeparam name="Value">消息头值类型</typeparam>
        /// <param name="name">头名称</param>
        /// <returns>消息头值</returns>
        Value GetHeader<Value>(string name);
        string GetHeader(string name);
        string Topic { get; }
        Key MsgKey { get; }
        T Value { get; }
    }
}