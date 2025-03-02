using System;

namespace RpcTaskModel.TaskItem.Model
{
    public class TaskItem
    {
        /// <summary>
        /// 任务项ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 任务项标题
        /// </summary>
        public string ItemTitle { get; set; }

        /// <summary>
        /// 任务排序号
        /// </summary>
        public int ItemSort { get; set; }

        /// <summary>
        /// 发送类型
        /// </summary>
        public TaskSendType SendType { get; set; }

        /// <summary>
        /// 任务超时时间
        /// </summary>
        public int? TimeOut { get; set; }

        /// <summary>
        /// 任务重试次数
        /// </summary>
        public short? RetryNum { get; set; }
        /// <summary>
        /// 失败后步骤
        /// </summary>
        public TaskStep FailStep { get; set; }
        /// <summary>
        /// 失败后执行的步骤
        /// </summary>
        public int? FailNextStep { get; set; }
        /// <summary>
        /// 成功的步骤
        /// </summary>
        public TaskStep SuccessStep { get; set; }
        /// <summary>
        /// 下一步的步骤
        /// </summary>
        public int? NextStep { get; set; }


        /// <summary>
        /// 日志记录范围
        /// </summary>
        public TaskLogRange LogRange { get; set; }

        /// <summary>
        /// 上一次执行是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        public string Error { get; set; }


        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime? LastExecTime { get; set; }
    }
}
