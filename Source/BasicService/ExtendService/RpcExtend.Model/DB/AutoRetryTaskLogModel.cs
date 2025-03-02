using SqlSugar;

namespace RpcExtend.Model.DB
{
    [SugarTable("AutoRetryTaskLog")]
    public class AutoRetryTaskLogModel
    {
        // <summary>
        /// 日志ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }
        /// <summary>
        /// 任务ID
        /// </summary>
        public long TaskId { get; set; }

        /// <summary>
        /// 执行任务的节点
        /// </summary>
        public long ServerId { get; set; }

        /// <summary>
        /// 是否失败
        /// </summary>
        public bool IsFail { get; set; }

        /// <summary>
        /// 当前次数
        /// </summary>
        public int RetryNum { get; set; }

        /// <summary>
        /// 最后次错误码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// 运行时间
        /// </summary>
        public DateTime RunTime { get; set; }
    }
}
