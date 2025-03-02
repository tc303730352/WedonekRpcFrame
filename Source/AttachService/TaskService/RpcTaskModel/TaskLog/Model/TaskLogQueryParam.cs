using System;
using WeDonekRpc.Helper.Validate;
namespace RpcTaskModel.TaskLog.Model
{
    public class TaskLogQueryParam
    {
        [NumValidate("rpc.task.Id.error", 1)]
        public long TaskId
        {
            get;
            set;
        }
        public long? ItemId
        {
            get;
            set;
        }
        public bool? IsFail
        {
            get;
            set;
        }
        public DateTime? Begin
        {
            get;
            set;
        }
        public DateTime? End
        {
            get;
            set;
        }
    }
}
