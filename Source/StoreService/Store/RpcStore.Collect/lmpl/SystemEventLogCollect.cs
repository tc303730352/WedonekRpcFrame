using WeDonekRpc.Model;
using RpcStore.DAL;
using RpcStore.Model.ExtendDB;
using RpcStore.Model.SysEventLog;
using RpcStore.RemoteModel.SysEventLog.Model;

namespace RpcStore.Collect.lmpl
{
    internal class SystemEventLogCollect : ISystemEventLogCollect
    {
        private readonly ISystemEventLogDAL _BasicDAL;

        public SystemEventLogCollect (ISystemEventLogDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }

        public SystemEventLogModel Get (long id)
        {
            return this._BasicDAL.Get(id);
        }

        public SystemEventLog[] Query (SysEventLogQuery query, IBasicPage paging, out int count)
        {
            return this._BasicDAL.Query(query, paging, out count);
        }
    }
}
