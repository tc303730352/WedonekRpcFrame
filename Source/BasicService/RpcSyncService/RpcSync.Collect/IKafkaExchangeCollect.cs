namespace RpcSync.Collect
{
    public interface IKafkaExchangeCollect
    {
        int SyncExchange(string exchange);
    }
}