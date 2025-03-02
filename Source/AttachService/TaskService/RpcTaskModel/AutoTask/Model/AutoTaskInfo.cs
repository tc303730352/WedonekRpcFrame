using System;

namespace RpcTaskModel.AutoTask.Model
{
    /// <summary>
    /// 任务信息
    /// </summary>
    public class AutoTaskInfo
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 区域ID
        /// </summary>
        public int? RegionId { get; set; }

        /// <summary>
        /// 集群ID
        /// </summary>
        public long RpcMerId { get; set; }

        /// <summary>
        /// 任务名
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// 任务说明
        /// </summary>
        public string TaskShow { get; set; }

        /// <summary>
        /// 任务优先级
        /// </summary>
        public int TaskPriority { get; set; }
        /// <summary>
        /// 开始步骤号
        /// </summary>
        public short BeginStep { get; set; }
        /// <summary>
        /// 执行失败时的通知邮件地址
        /// </summary>
        public string[] FailEmall { get; set; }

        /// <summary>
        /// 任务版本号
        /// </summary>
        public int VerNum { get; set; }

        /// <summary>
        /// 是否执行中
        /// </summary>
        public bool IsExec { get; set; }

        /// <summary>
        /// 执行版本号
        /// </summary>
        public int ExecVerNum { get; set; }

        /// <summary>
        /// 最后执行时间
        /// </summary>
        public DateTime? LastExecTime { get; set; }
        /// <summary>
        /// 最后执行结束时间
        /// </summary>
        public DateTime? LastExecEndTime { get; set; }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime? NextExecTime { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public AutoTaskStatus TaskStatus { get; set; }

        /// <summary>
        /// 任务错误码ID
        /// </summary>
        public long? ErrorId { get; set; }

        /// <summary>
        /// 停用时间
        /// </summary>
        public DateTime? StopTime { get; set; }
    }
}
