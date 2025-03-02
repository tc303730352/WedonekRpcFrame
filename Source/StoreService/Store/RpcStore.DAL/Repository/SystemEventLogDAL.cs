using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.Model.SysEventLog;
using RpcStore.RemoteModel.SysEventLog.Model;

namespace RpcStore.DAL.Repository
{
    internal class SystemEventLogDAL : ISystemEventLogDAL
    {
        private readonly IRpcExtendResource<SystemEventLogModel> _BasicDAL;
        public SystemEventLogDAL (IRpcExtendResource<SystemEventLogModel> dal)
        {
            this._BasicDAL = dal;
        }
        public SystemEventLog[] Query (SysEventLogQuery query, IBasicPage paging, out int count)
        {
            paging.InitOrderBy("Id", true);
            return this._BasicDAL.Query<SystemEventLog>(query.ToWhere(), paging, out count);
        }
        public SystemEventLogModel Get (long id)
        {
            SystemEventLogModel obj = this._BasicDAL.Get(a => a.Id == id);
            if (obj == null)
            {
                throw new ErrorException("rpc.store.event.log.not.find");
            }
            return obj;
        }
    }
}
