using RpcStore.Model.ExtendDB;

namespace RpcStore.DAL.Repository
{
    internal class AutoRetryTaskLogDAL : IAutoRetryTaskLogDAL
    {
        private readonly IRpcExtendResource<AutoRetryTaskLogModel> _BasicDAL;
        public AutoRetryTaskLogDAL (IRpcExtendResource<AutoRetryTaskLogModel> dal)
        {
            this._BasicDAL = dal;
        }
        public AutoRetryTaskLogModel[] GetLogs (long taskId)
        {
            return this._BasicDAL.Gets(a => a.TaskId == taskId);
        }
    }
}
