using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.Model.SysEventLog;
using RpcStore.RemoteModel.SysEventLog.Model;

namespace RpcStore.Collect
{
    public interface ISystemEventLogCollect
    {
        SystemEventLogModel Get (long id);
        SystemEventLog[] Query (SysEventLogQuery query, IBasicPage paging, out int count);
    }
}