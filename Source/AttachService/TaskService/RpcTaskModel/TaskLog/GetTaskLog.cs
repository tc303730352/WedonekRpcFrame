using WeDonekRpc.Client;

namespace RpcTaskModel.TaskLog
{
    [WeDonekRpc.Model.IRemoteConfig("sys.task")]
    public class GetTaskLog : RpcRemote<Model.TaskLogDetailed>
    {
        public long Id { get; set; }
    }
}
