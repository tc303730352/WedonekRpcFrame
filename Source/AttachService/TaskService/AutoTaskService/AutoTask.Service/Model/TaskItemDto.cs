using RpcTaskModel;
using RpcTaskModel.TaskItem.Model;
using SqlSugar;

namespace AutoTask.Service.Model
{
    public class TaskItemDto
    {
        public long Id { get; set; }

        /// <summary>
        /// 计划名
        /// </summary>
        public string ItemTitle { get; set; }

        /// <summary>
        /// 排序位
        /// </summary>
        public int ItemSort { get; set; }
        /// <summary>
        /// 发送类型
        /// </summary>
        public TaskSendType SendType { get; set; }
        /// <summary>
        /// 发送参数
        /// </summary>
        public SendParam SendParam { get; set; }
        /// <summary>
        /// 超时时间
        /// </summary>
        public int? TimeOut { get; set; }
        /// <summary>
        /// 重试次数
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
        /// 成功后步骤
        /// </summary>
        public TaskStep SuccessStep { get; set; }
        /// <summary>
        /// 下一步
        /// </summary>
        public int? NextStep { get; set; }

        /// <summary>
        /// 成功是否写入日志
        /// </summary>
        public TaskLogRange LogRange { get; set; }

        public string ClientKey
        {
            get;
            set;
        }
    }
}
