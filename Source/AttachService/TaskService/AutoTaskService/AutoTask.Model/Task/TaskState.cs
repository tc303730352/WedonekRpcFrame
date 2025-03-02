using RpcTaskModel;

namespace AutoTask.Model.Task
{
    public class TaskState
    {
        /// <summary>
        /// 任务状态
        /// </summary>
        public AutoTaskStatus TaskStatus { get; set; }
        /// <summary>
        /// 执行版本号
        /// </summary>
        public int ExecVerNum { get; set; }
        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime? NextExecTime
        {
            get;
            set;
        }
    }
}
