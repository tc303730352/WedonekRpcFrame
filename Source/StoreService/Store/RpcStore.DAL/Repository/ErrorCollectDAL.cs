using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.Error.Model;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class ErrorCollectDAL : IErrorCollectDAL
    {
        private readonly IRepository<ErrorCollectModel> _BasicDAL;
        public ErrorCollectDAL (IRepository<ErrorCollectModel> dal)
        {
            this._BasicDAL = dal;
        }
        public Dictionary<long, string> GetErrorCode (long[] errorId)
        {
            return this._BasicDAL.Gets(a => errorId.Contains(a.Id), a => new
            {
                a.Id,
                a.ErrorCode
            }).ToDictionary(a => a.Id, a => a.ErrorCode);
        }
        public ErrorCollectModel[] Query (ErrorQuery query, IBasicPage paging, out int count)
        {
            paging.InitOrderBy("Id", true);
            return this._BasicDAL.Query(query.ToWhere(), paging, out count);
        }
        public ErrorCollectModel Get (long id)
        {
            return this._BasicDAL.Get(c => c.Id == id);
        }
        public ErrorCollectModel Find (string code)
        {
            return this._BasicDAL.Get(c => c.ErrorCode == code);
        }

        public void SetIsPerfect (long id)
        {
            if (!this._BasicDAL.Update(c => c.IsPerfect == true, c => c.Id == id))
            {
                throw new ErrorException("rpc.store.error.perfect.set.error");
            }
        }

        public void Add (ErrorCollectModel add)
        {
            add.Id = IdentityHelper.CreateId();
            this._BasicDAL.Insert(add);
        }

        public ErrorCollectModel[] Gets (long[] ids)
        {
            return this._BasicDAL.Gets(c => ids.Contains(c.Id));
        }
    }
}
