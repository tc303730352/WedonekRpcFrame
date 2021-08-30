using RpcHelper;

namespace RpcCacheClient.Interface
{
        public interface ISubscriberController : IDataSyncClass
        {
                string SubName { get; }
        }
}