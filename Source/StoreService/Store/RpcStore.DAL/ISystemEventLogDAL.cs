using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.Model.SysEventLog;
using RpcStore.RemoteModel.SysEventLog.Model;

namespace RpcStore.DAL
{
    public interface ISystemEventLogDAL
    {
        SystemEventLogModel Get (long id);
        SystemEventLog[] Query (SysEventLogQuery query, IBasicPage paging, out int count);
    }
}