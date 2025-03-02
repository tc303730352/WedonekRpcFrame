using WeDonekRpc.Helper.Interface;

namespace WeDonekRpc.CacheClient.Interface
{
    public interface ISubscriberController : IDataSyncClass
    {
        string SubName { get; }
    }
}