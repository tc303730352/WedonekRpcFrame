using WeDonekRpc.Helper.Validate;

namespace RpcTaskModel.TaskItem.Model
{
    public class TaskItemSetParam
    {
        /// <summary>
        /// 任务项标题
        /// </summary>
        [NullValidate("task.item.title.null")]
        [LenValidate("task.item.title.len", 2, 50)]
        public string ItemTitle
        {
            get;
            set;
        }
        /// <summary>
        /// 排序位
        /// </summary>
        [NumValidate("task.item.sort.error", 0)]
        public int ItemSort
        {
            get;
            set;
        }
        /// <summary>
        /// 发送类型
        /// </summary>
        [EnumValidate("task.item.sendType.error", typeof(TaskSendType))]
        public TaskSendType SendType
        {
            get;
            set;
        }
        /// <summary>
        /// 发送参数
        /// </summary>
        [NullValidate("task.item.send.param.null")]
        [EntrustValidate("_Check")]
        public SendParam SendParam
        {
            get;
            set;
        }
        /// <summary>
        /// 超时时间(秒)
        /// </summary>
        [NumValidate("task.item.timeout.error", 5)]
        public int? TimeOut
        {
            get;
            set;
        }
        /// <summary>
        /// 允许重试次数
        /// </summary>
        [NumValidate("task.item.retry.num.error", 0)]
        public short? RetryNum
        {
            get;
            set;
        }
        /// <summary>
        /// 执行失败的步骤
        /// </summary>
        [EnumValidate("task.item.fail.step.error", typeof(TaskStep))]
        public TaskStep FailStep
        {
            get;
            set;
        }
        /// <summary>
        /// 失败后的步骤
        /// </summary>
        [NumValidate("task.item.fail.stepNum.error", 1)]
        public int? FailNextStep
        {
            get;
            set;
        }
        /// <summary>
        /// 成功步骤
        /// </summary>
        [EnumValidate("task.item.success.step.error", typeof(TaskStep))]
        public TaskStep SuccessStep
        {
            get;
            set;
        }
        /// <summary>
        /// 成功后下一步骤
        /// </summary>
        [NumValidate("task.item.next.step.error", 1)]
        public int? NextStep
        {
            get;
            set;
        }
        /// <summary>
        /// 日志范围
        /// </summary>
        public TaskLogRange LogRange { get; set; }


        private bool _Check (out string error)
        {
            if (this.SendType == TaskSendType.指令 && this.SendParam.RpcConfig == null)
            {
                error = "task.item.rpc.config.null";
                return false;
            }
            else if (this.SendType == TaskSendType.广播 && this.SendParam.BroadcastConfig == null)
            {
                error = "task.item.rpc.broadcast.config.null";
                return false;
            }
            else if (this.SendType == TaskSendType.Http && this.SendParam.HttpConfig == null)
            {
                error = "task.item.http.config.null";
                return false;
            }
            else if (this.FailStep == TaskStep.执行指定步骤 && !this.FailNextStep.HasValue)
            {
                error = "task.item.fail.step.null";
                return false;
            }
            else if (this.FailStep == TaskStep.继续下一步 && this.SuccessStep == TaskStep.停止执行)
            {
                //不允许在结束节点上 失败步骤不允许下一步
                error = "task.item.fail.step.set.error";
                return false;
            }
            else if (this.SuccessStep == TaskStep.执行指定步骤 && !this.NextStep.HasValue)
            {
                error = "task.item.next.step.null";
                return false;
            }
            error = null;
            return true;
        }
    }
}
