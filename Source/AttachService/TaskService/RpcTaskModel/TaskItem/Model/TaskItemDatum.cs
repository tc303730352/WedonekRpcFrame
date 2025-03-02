using System;

namespace RpcTaskModel.TaskItem.Model
{
    public class TaskItemDatum
    {
        /// <summary>
        /// 任务项ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 任务ID
        /// </summary>
        public long TaskId { get; set; }
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
        /// 任务发送参数
        /// </summary>
        public SendParam SendParam { get; set; }

        /// <summary>
        /// 任务超时时间
        /// </summary>
        public int? TimeOut { get; set; }

        /// <summary>
        /// 任务重试次数
        /// </summary>
        public short? RetryNum { get; set; }

        /// <summary>
        /// 失败时步骤
        /// </summary>
        public TaskStep FailStep { get; set; }

        /// <summary>
        /// 失败时执行的下一步
        /// </summary>
        public int? FailNextStep { get; set; }
        /// <summary>
        /// 成功时步骤
        /// </summary>
        public TaskStep SuccessStep { get; set; }
        /// <summary>
        /// 成功时执行的下一步
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
        /// <summary>
        /// 错误ID
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime? LastExecTime { get; set; }
    }
}
