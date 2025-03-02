using RpcSync.Model;

namespace RpcSync.DAL
{
    public interface IServerEventSwitchDAL
    {
        EventSwitch[] GetsEnableEvent ( long serverId, long rpcMerId, string module );
        EventSwitch[] GetsEnableEvent ( long serverId, long rpcMerId );
        ServiceEventDatum[] Gets ( long[] ids );
    }
}