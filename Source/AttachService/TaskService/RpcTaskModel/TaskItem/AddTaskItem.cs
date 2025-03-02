using WeDonekRpc.Client;
using RpcTaskModel.TaskItem.Model;

namespace RpcTaskModel.TaskItem
{
    [WeDonekRpc.Model.IRemoteConfig("sys.task")]
    public class AddTaskItem : RpcRemote<long>
    {
        public long TaskId
        {
            get;
            set;
        }
        public TaskItemSetParam Dataum
        {
            get;
            set;
        }
    }
}
