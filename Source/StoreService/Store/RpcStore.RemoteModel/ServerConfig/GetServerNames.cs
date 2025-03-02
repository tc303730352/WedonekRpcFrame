using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerConfig
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetServerNames : RpcRemote<Dictionary<long, string>>
    {
        public long[] ServerId
        {
            get;
            set;
        }
    }
}
