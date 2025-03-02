using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerBind.Model;

namespace RpcStore.RemoteModel.ServerBind
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetBindServerGroupType : RpcRemoteArray<BindServerGroupType>
    {
        public BindGetParam Param { get; set; }
    }
}
