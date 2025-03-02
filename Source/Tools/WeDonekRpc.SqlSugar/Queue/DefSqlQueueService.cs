namespace WeDonekRpc.SqlSugar.Queue
{
    internal class DefSqlQueueService : SqlQueueService
    {
        public DefSqlQueueService(ISqlClientFactory factory) : base(factory, "default")
        {
        }
    }
}
