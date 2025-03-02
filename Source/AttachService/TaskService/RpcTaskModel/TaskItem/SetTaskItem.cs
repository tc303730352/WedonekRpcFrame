using WeDonekRpc.Client;
using RpcTaskModel.TaskItem.Model;

namespace RpcTaskModel.TaskItem
{
    [WeDonekRpc.Model.IRemoteConfig("sys.task")]
    public class SetTaskItem : RpcRemote<bool>
    {
        public long ItemId
        {
            get;
            set;
        }
        public TaskItemSetParam Datum
        {
            get;
            set;
        }
    }
}
