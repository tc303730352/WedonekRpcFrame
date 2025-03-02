using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerEventSwitch.Model;

namespace RpcStore.RemoteModel.ServerEventSwitch
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class AddEventSwitch : RpcRemote<long>
    {
        public EventSwitchAdd Datum { get; set; }
    }
}
