using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerVer
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetServerVer : RpcRemote
    {
        public long RpcMerId { get; set; }

        public long SystemTypeId { get; set; }

        public int VerNum { get; set; }
    }
}
