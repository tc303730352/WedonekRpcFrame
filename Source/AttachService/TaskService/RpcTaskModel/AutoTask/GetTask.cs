using WeDonekRpc.Client;
using RpcTaskModel.AutoTask.Model;

namespace RpcTaskModel.AutoTask
{
    [WeDonekRpc.Model.IRemoteConfig("sys.task")]
    public class GetTask : RpcRemote<AutoTaskInfo>
    {
        public long TaskId { get; set; }
    }
}
