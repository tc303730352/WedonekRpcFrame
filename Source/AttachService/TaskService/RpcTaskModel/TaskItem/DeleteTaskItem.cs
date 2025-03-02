using WeDonekRpc.Client;

namespace RpcTaskModel.TaskItem
{
    [WeDonekRpc.Model.IRemoteConfig("sys.task")]
    public class DeleteTaskItem : RpcRemote
    {
        public long ItemId
        {
            get;
            set;
        }
    }
}
