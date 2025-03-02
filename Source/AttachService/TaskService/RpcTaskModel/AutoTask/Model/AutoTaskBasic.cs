using System;

namespace RpcTaskModel.AutoTask.Model
{
    /// <summary>
    /// 任务信息
    /// </summary>
    public class AutoTaskBasic
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
        /// 任务名称
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
        /// 是否在执行中
        /// </summary>
        public bool IsExec { get; set; }

        /// <summary>
        /// 最后执行开始时间
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
        /// 停止使用时间
        /// </summary>
        public DateTime? StopTime { get; set; }
    }
}
