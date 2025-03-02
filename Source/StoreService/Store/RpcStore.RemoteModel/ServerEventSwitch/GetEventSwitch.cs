using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerEventSwitch.Model;

namespace RpcStore.RemoteModel.ServerEventSwitch
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetEventSwitch : RpcRemote<EventSwitchData>
    {
        public long Id { get; set; }
    }
}
