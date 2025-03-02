using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.Model.SysError;
using RpcStore.RemoteModel.SysLog.Model;

namespace RpcStore.DAL.Repository
{
    internal class SystemLogDAL : ISystemLogDAL
    {
        private readonly IRpcExtendResource<SystemErrorLogModel> _BasicDAL;
        public SystemLogDAL (IRpcExtendResource<SystemErrorLogModel> dal)
        {
            this._BasicDAL = dal;
        }
        public SystemErrorLogModel Get (long id)
        {
            return this._BasicDAL.Get(c => c.Id == id);
        }
        public SysErrorLog[] Query (SysLogQuery query, IBasicPage paging, out int count)
        {
            paging.InitOrderBy("Id", true);
            return this._BasicDAL.Query<SysErrorLog>(query.ToWhere(), paging, out count);
        }
    }
}
