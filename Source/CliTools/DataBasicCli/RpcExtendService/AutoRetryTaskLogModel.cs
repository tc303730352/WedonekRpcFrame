using SqlSugar;

namespace DataBasicCli.RpcExtendService
{
    [SugarTable("AutoRetryTaskLog")]
    public class AutoRetryTaskLogModel
    {
        // <summary>
        /// 日志ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "日志ID")]
        public long Id { get; set; }
        /// <summary>
        /// 任务ID
        /// </summary>
        [SugarColumn(ColumnDescription = "任务ID")]
        public long TaskId { get; set; }

        /// <summary>
        /// 执行的节点
        /// </summary>
        [SugarColumn(ColumnDescription = "执行的节点")]
        public long ServerId { get; set; }

        /// <summary>
        /// 是否失败
        /// </summary>
        [SugarColumn(ColumnDescription = "是否失败")]
        public bool IsFail { get; set; }


        /// <summary>
        /// 当前次数
        /// </summary>
        [SugarColumn(ColumnDescription = "当前次数")]
        public int RetryNum { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        [SugarColumn(ColumnDescription = "错误码", ColumnDataType = "varchar", Length = 100)]
        public string ErrorCode { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        [SugarColumn(ColumnDescription = "执行时长")]
        public int Duration { get; set; }
        /// <summary>
        /// 运行时间
        /// </summary>
        [SugarColumn(ColumnDescription = "执行开始时间")]
        public DateTime RunTime { get; set; }
    }
}
