using RpcStore.Model.AutoRetryTask;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.RetryTask.Model;
using WeDonekRpc.Model;

namespace RpcStore.DAL.Repository
{
    internal class AutoRetryTaskDAL : IAutoRetryTaskDAL
    {
        private readonly IRpcExtendResource<AutoRetryTaskModel> _BasicDAL;
        public AutoRetryTaskDAL (IRpcExtendResource<AutoRetryTaskModel> dal)
        {
            this._BasicDAL = dal;
        }
        public RetryTask[] Query (RetryTaskQuery query, IBasicPage paging, out int count)
        {
            paging.InitOrderBy("Id", true);
            return this._BasicDAL.Query<RetryTask>(query.ToWhere(), paging, out count);
        }

        public AutoRetryTaskModel Get (long id)
        {
            return this._BasicDAL.Get(a => a.Id == id);
        }

        public RetryTaskState[] GetTaskStates (long[] ids)
        {
            return this._BasicDAL.Gets<RetryTaskState>(a => ids.Contains(a.Id));
        }
        public RetryTaskState GetTaskState (long id)
        {
            return this._BasicDAL.Get<RetryTaskState>(a => a.Id == id);
        }
    }
}
