using RpcSync.Model;

namespace RpcSync.Collect
{
    public interface IServerEventCollect
    {
        ServiceEventDatum[] Gets ( long[] ids );
        EventSwitch[] GetsEnableEvent ( long serverId, long rpcMerId );
        EventSwitch[] GetsEnableEvent ( long serverId, long rpcMerId, string module );
        void Refresh ( long[] serverId, string module );
    }
}