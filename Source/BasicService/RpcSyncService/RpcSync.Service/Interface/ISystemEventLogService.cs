using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.SysEvent.Model;

namespace RpcSync.Service.Interface
{
    public interface ISystemEventLogService
    {
        void Add(SysEventLog[] logs, MsgSource source);
    }
}