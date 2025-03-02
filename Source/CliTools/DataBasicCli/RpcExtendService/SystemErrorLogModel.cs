using SqlSugar;

namespace DataBasicCli.RpcExtendService
{
    [SugarTable("SystemErrorLog", "系统日志表")]
    public class SystemErrorLogModel
    {
        /// <summary>
        /// 日志Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "日志Id")]
        public long Id
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "集群ID")]
        public long RpcMerId
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "关联的链路ID", Length = 32)]
        public string TraceId
        {
            get;
            set;
        }

        /// <summary>
        /// 日志标题
        /// </summary>
        [SugarColumn(ColumnDescription = "日志标题", Length = 200)]
        public string LogTitle
        {
            get;
            set;
        }

        /// <summary>
        /// 日志说明
        /// </summary>
        [SugarColumn(ColumnDescription = "日志说明", Length = 255)]
        public string LogShow
        {
            get;
            set;
        }

        /// <summary>
        /// 所属系统类别ID
        /// </summary>
        [SugarColumn(ColumnDescription = "服务类别值", Length = 50, ColumnDataType = "varchar")]
        public string SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点
        /// </summary>
        [SugarColumn(ColumnDescription = "来源节点")]
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 日志组别
        /// </summary>
        [SugarColumn(ColumnDescription = "日志组别", Length = 50, ColumnDataType = "varchar")]
        public string LogGroup
        {
            get;
            set;
        }
        /// <summary>
        /// 日志类型
        /// </summary>
        [SugarColumn(ColumnDescription = "日志类型")]
        public byte LogType
        {
            get;
            set;
        }
        /// <summary>
        /// 日志等级
        /// </summary>
        [SugarColumn(ColumnDescription = "日志等级")]
        public byte LogGrade
        {
            get;
            set;
        }
        /// <summary>
        /// 错误码
        /// </summary>
        [SugarColumn(ColumnDescription = "错误码", Length = 100, ColumnDataType = "varchar")]
        public string ErrorCode
        {
            get;
            set;
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        [SugarColumn(ColumnDescription = "错误信息", IsNullable = true)]
        public string Exception
        {
            get;
            set;
        }
        /// <summary>
        /// 属性列表
        /// </summary>
        [SugarColumn(ColumnDescription = "属性列表", ColumnDataType = "text", IsNullable = true)]
        public string AttrList
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "添加时间")]
        public DateTime AddTime
        {
            get;
            set;
        }
    }
}
