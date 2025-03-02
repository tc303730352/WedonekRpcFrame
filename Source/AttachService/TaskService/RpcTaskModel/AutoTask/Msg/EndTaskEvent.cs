using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace RpcTaskModel.AutoTask.Msg
{
    [IRemoteBroadcast("EndTask", true, "sys.task", IsCrossGroup = true, IsExclude = false)]
    public class EndTaskEvent : RpcBroadcast
    {
        public long TaskId
        {
            get;
            set;
        }

        public bool IsEnd
        {
            get;
            set;
        }
    }
}
