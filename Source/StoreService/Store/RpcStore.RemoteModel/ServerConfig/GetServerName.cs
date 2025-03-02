using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerConfig
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetServerName : RpcRemote<string>
    {
        public long ServerId
        {
            get;
            set;
        }
    }
}
