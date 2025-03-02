using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerEventSwitch.Model;

namespace RpcStore.RemoteModel.ServerEventSwitch
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QueryEventSwitch : BasicPage<EventSwitch>
    {
        public EventSwitchQuery Query { get; set; }
    }
}
