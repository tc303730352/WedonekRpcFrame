using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerEventSwitch
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetEventSwitchIsEnable : RpcRemote
    {
        public long Id { get; set; }

        public bool IsEnable { get; set; }
    }
}
