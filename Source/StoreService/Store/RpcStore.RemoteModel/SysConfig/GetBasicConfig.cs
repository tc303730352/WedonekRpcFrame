using WeDonekRpc.Client;
using RpcStore.RemoteModel.SysConfig.Model;

namespace RpcStore.RemoteModel.SysConfig
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetBasicConfig : RpcRemote<BasicSysConfig>
    {
        public BasicGetParam GetParam { get; set; }
    }
}
