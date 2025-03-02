using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace RpcTaskModel.AutoTask
{
    [IRemoteConfig("sys.task")]
    public class EnableTask : RpcRemote<bool>
    {
        public long TaskId { get; set; }
    }
}
