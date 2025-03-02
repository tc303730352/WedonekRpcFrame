using System;

namespace RpcTaskModel.TaskLog.Model
{
    public class TaskLogDatum
    {
        /// <summary>
        /// 任务日志Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 任务项
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// 任务项名字
        /// </summary>
        public string ItemTitle { get; set; }

        /// <summary>
        /// 是否失败
        /// </summary>
        public bool IsFail { get; set; }

        /// <summary>
        /// 失败的错误码
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// 开始执行的时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结束执行时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}
