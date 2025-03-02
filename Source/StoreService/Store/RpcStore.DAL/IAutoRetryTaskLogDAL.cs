using RpcStore.Model.ExtendDB;

namespace RpcStore.DAL
{
    public interface IAutoRetryTaskLogDAL
    {
        AutoRetryTaskLogModel[] GetLogs ( long taskId );
    }
}