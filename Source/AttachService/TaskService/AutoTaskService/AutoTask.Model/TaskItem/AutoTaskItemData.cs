using RpcTaskModel;

namespace AutoTask.Model.TaskItem
{
    public class AutoTaskItemData
    {
        public long Id { get; set; }

        public string ItemTitle { get; set; }

        public int ItemSort { get; set; }

        public TaskSendType SendType { get; set; }

        public int? TimeOut { get; set; }

        public short? RetryNum { get; set; }

        public TaskStep FailStep { get; set; }

        public int? FailNextStep { get; set; }

        public TaskStep SuccessStep { get; set; }

        public int? NextStep { get; set; }

        public TaskLogRange LogRange { get; set; }

        public bool IsSuccess { get; set; }

        public string Error { get; set; }

        public DateTime? LastExecTime { get; set; }
    }
}
