using System;
using WeDonekRpc.Helper.Validate;

namespace RpcTaskModel.AutoTask.Model
{
    public class TaskQueryParam
    {
        /// <summary>
        /// 查询关键字
        /// </summary>
        public string QueryKey { get; set; }
        /// <summary>
        /// 机房ID
        /// </summary>
        public int? RegionId { get; set; }

        /// <summary>
        /// 所属集群
        /// </summary>
        [NumValidate("task.mer.id.error", 1)]
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否在执行中
        /// </summary>
        public bool? IsExec { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public AutoTaskStatus[] TaskStatus { get; set; }
        /// <summary>
        /// 执行开始时间
        /// </summary>
        public DateTime? Begin { get; set; }
        /// <summary>
        /// 执行结束时间
        /// </summary>
        public DateTime? End { get; set; }
    }
}
