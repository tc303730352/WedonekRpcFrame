using RpcTaskModel;
using RpcTaskModel.TaskItem.Model;
using SqlSugar;

namespace AutoTask.Model.TaskItem
{
    public class TaskSetItem
    {
        /// <summary>
        /// 任务项标题
        /// </summary>
        public string ItemTitle
        {
            get;
            set;
        }
        /// <summary>
        /// 排序位
        /// </summary> 
        public int ItemSort
        {
            get;
            set;
        }
        /// <summary>
        /// 发送类型
        /// </summary> 
        public TaskSendType SendType
        {
            get;
            set;
        }
        /// <summary>
        /// 发送参数
        /// </summary> 
        [SugarColumn(IsJson = true, ColumnDataType = "nvarchar(max)")]
        public SendParam SendParam
        {
            get;
            set;
        }
        /// <summary>
        /// 超时时间(秒)
        /// </summary>
        public int? TimeOut
        {
            get;
            set;
        }
        /// <summary>
        /// 允许重试次数
        /// </summary>
        public short? RetryNum
        {
            get;
            set;
        }
        /// <summary>
        /// 执行失败的步骤
        /// </summary>
        public TaskStep FailStep
        {
            get;
            set;
        }
        /// <summary>
        /// 失败后的步骤
        /// </summary>
        public int? FailNextStep
        {
            get;
            set;
        }
        /// <summary>
        /// 成功步骤
        /// </summary>
        public TaskStep SuccessStep
        {
            get;
            set;
        }
        /// <summary>
        /// 成功后下一步骤
        /// </summary>
        public int? NextStep
        {
            get;
            set;
        }
        /// <summary>
        /// 日志范围
        /// </summary>
        public TaskLogRange LogRange { get; set; }
    }
}
