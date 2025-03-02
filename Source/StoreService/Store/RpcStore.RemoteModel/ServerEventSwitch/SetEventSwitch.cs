using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerEventSwitch.Model;

namespace RpcStore.RemoteModel.ServerEventSwitch
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetEventSwitch : RpcRemote<bool>
    {
        public long Id { get; set; }
        public EventSwitchSet Datum { get; set; }
    }
}
