using RpcTaskModel;

namespace AutoTask.Collect.Model
{
    /// <summary>
    /// 任务执行日志
    /// </summary>
    public class TaskExecLog
    {
        /// <summary>
        /// 是否失败
        /// </summary>
        public bool isFail;
        /// <summary>
        /// 错误Id
        /// </summary>
        public string error;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime begin;
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime end;
        /// <summary>
        /// 结果
        /// </summary>
        public string result;
        /// <summary>
        /// 当前重试次数
        /// </summary>
        public int retryNum;
        /// <summary>
        /// 日志范围
        /// </summary>
        public TaskLogRange logRange;
    }
}
