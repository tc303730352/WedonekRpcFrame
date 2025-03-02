using SqlSugar;

namespace DataBasicCli.RpcExtendService
{
    [SugarTable("SystemEventLog", "服务节点事件日志表")]
    public class SystemEventLogModel
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "日志ID")]
        public long Id
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "来源集群Id")]
        public long RpcMerId
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "来源服务节点Id")]
        public long ServerId
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "来源服务节点类别Id")]
        public long SystemTypeId
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "来源机房ID")]
        public int RegionId
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "来源事件源ID")]
        public int EventSourceId
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "事件名", Length = 50)]
        public string EventName { get; set; }

        [SugarColumn(ColumnDescription = "事件级别")]
        public byte EventLevel
        {
            get;
            set;
        }

        [SugarColumn(ColumnDescription = "事件类别")]
        public byte EventType
        {
            get;
            set;
        }
        /// <summary>
        /// 事件的说明
        /// </summary>
        [SugarColumn(ColumnDescription = "事件说明", Length = 500)]
        public string EventShow
        {
            get;
            set;
        }
        /// <summary>
        /// 事件属性
        /// </summary>
        [SugarColumn(ColumnDescription = "事件属性")]
        public string EventAttr
        {
            get;
            set;
        }
        /// <summary>
        /// 事件时间
        /// </summary>
        [SugarColumn(ColumnDescription = "添加时间")]
        public DateTime AddTime
        {
            get;
            set;
        }
    }
}
