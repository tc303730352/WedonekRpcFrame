using RpcTaskModel;
using RpcTaskModel.TaskItem.Model;

namespace AutoTask.Gateway.Model
{
    public class AutoTaskItem
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
        /// Http参数
        /// </summary>
        public HttpParamConfig HttpParam { get; set; }

        /// <summary>
        /// 任务Rpc参数
        /// </summary>
        public TaskRpcParam RpcParam { get; set; }
        /// <summary>
        /// 广播参数
        /// </summary>
        public TaskRpcBroadcast BroadcastParam {  get; set; }
        /// <summary>
        /// 远程配置
        /// </summary>
        public RpcRemoteSet RemoteSet
        {
            get;
            set;
        }
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
        public long? ErrorId { get; set; }
        /// <summary>
        /// 错误码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime? LastExecTime { get; set; }
    }
}
