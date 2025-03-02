using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerBind
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class CheckIsBindServer : RpcRemoteArray<long>
    {
        public long RpcMerId { get; set; }

        public long[] ServerId { get; set; }
    }
}
