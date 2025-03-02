using SqlSugar;

namespace WeDonekRpc.SqlSugar.Queue
{
    public class SqlQueueService : ISqlQueueService
    {
        private ISqlClientFactory _Factory;
        private string _ConfigId;
        public SqlQueueService(ISqlClientFactory factory, string configId)
        {
            _ConfigId = configId;
            _Factory = factory;
        }
        public ISqlQueue BeginQueue()
        {
            SqlSugarProvider provider = this._Factory.GetProvider(_ConfigId);
            return new LocalQueue(provider);
        }
    }
}
