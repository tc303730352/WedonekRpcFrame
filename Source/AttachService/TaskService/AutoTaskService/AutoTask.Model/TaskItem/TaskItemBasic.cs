using RpcTaskModel;

namespace AutoTask.Model.TaskItem
{
    public class TaskItemBasic
    {
        public long Id { get; set; }
        public string ItemTitle { get; set; }

        public int ItemSort { get; set; }


        public TaskStep FailStep { get; set; }

        public int? FailNextStep { get; set; }

        public TaskStep SuccessStep { get; set; }

        public int? NextStep { get; set; }
    }
}
