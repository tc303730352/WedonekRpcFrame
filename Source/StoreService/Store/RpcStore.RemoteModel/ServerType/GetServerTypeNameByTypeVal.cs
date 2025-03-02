using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerType
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetServerTypeNameByTypeVal : RpcRemote<string>
    {
        public string TypeVal
        {
            get;
            set;
        }
    }
}
