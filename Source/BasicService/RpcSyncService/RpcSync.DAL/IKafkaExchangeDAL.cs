namespace RpcSync.DAL
{
    public interface IKafkaExchangeDAL
    {
        int SyncExchange(string exchange);
    }
}