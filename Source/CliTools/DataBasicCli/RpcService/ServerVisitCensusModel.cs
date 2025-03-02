using SqlSugar;

namespace DataBasicCli.RpcService
{
    [SugarIndex("IX_ServiceId", "ServiceId", OrderByType.Asc, false)]
    [SugarIndex("IX_ServiceId_Dictate", "ServiceId", OrderByType.Asc, "Dictate", OrderByType.Asc, true)]
    [SugarTable("ServerVisitCensus", "服务节点访问统计表")]
    public class ServerVisitCensusModel
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "标识ID")]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点
        /// </summary>
        [SugarColumn(ColumnDescription = "服务端口")]
        public long ServiceId
        {
            get;
            set;
        }
        /// <summary>
        /// 指令集
        /// </summary>
        [SugarColumn(ColumnDescription = "指令", Length = 50, ColumnDataType = "varchar")]
        public string Dictate
        {
            get;
            set;
        }
        /// <summary>
        /// 说明
        /// </summary>
        [SugarColumn(ColumnDescription = "说明", Length = 255)]
        public string Show
        {
            get;
            set;
        }
        /// <summary>
        /// 访问量
        /// </summary>
        [SugarColumn(ColumnDescription = "访问量")]
        public long VisitNum
        {
            get;
            set;
        }
        /// <summary>
        /// 成功数
        /// </summary>
        [SugarColumn(ColumnDescription = "成功数")]
        public long SuccessNum
        {
            get;
            set;
        }
        /// <summary>
        /// 失败数
        /// </summary>
        [SugarColumn(ColumnDescription = "失败数")]
        public long FailNum
        {
            get;
            set;
        }
        /// <summary>
        /// 今日访问数
        /// </summary>
        [SugarColumn(ColumnDescription = "今日访问数")]
        public long TodayVisit
        {
            get;
            set;
        }
        /// <summary>
        /// 今日失败数
        /// </summary>
        [SugarColumn(ColumnDescription = "今日失败数")]
        public long TodayFail
        {
            get;
            set;
        }
        /// <summary>
        /// 今日成功数
        /// </summary>
        [SugarColumn(ColumnDescription = "今日成功数")]
        public long TodaySuccess
        {
            get;
            set;
        }
    }
}
