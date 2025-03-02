using RpcStore.Model.ExtendDB;

namespace RpcStore.Collect
{
    public interface IAutoRetryTaskLogCollect
    {
        AutoRetryTaskLogModel[] GetLogs (long taskId);
    }
}