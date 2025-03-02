using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.SysEvent.Model;

namespace RpcSync.Service.Interface
{
    public interface ISysEventService
    {
        void Refresh (long rpcMerId, string module);
        ServiceSysEvent[] GetEnableEvents (MsgSource source, string module);
    }
}