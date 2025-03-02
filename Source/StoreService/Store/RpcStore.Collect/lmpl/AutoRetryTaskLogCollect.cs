using RpcStore.DAL;
using RpcStore.Model.ExtendDB;

namespace RpcStore.Collect.lmpl
{
    internal class AutoRetryTaskLogCollect : IAutoRetryTaskLogCollect
    {
        private readonly IAutoRetryTaskLogDAL _LogDAL;

        public AutoRetryTaskLogCollect (IAutoRetryTaskLogDAL logDAL)
        {
            this._LogDAL = logDAL;
        }
        public AutoRetryTaskLogModel[] GetLogs (long taskId)
        {
            return this._LogDAL.GetLogs(taskId);
        }
    }
}
