using WeDonekRpc.Client;

namespace RpcTaskModel.AutoTask
{
    [WeDonekRpc.Model.IRemoteConfig("sys.task")]
    public class DeleteTask : RpcRemote
    {
        public long TaskId { get; set; }
    }
}
