using SqlSugar;

namespace DataBasicCli.RpcExtendService
{
    [SugarTable("ServerEventSwitch")]
    public class ServerEventSwitchModel
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "标识Id")]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 集群ID
        /// </summary>
        [SugarColumn(ColumnDescription = "集群ID")]
        public long RpcMerId { get; set; }
        /// <summary>
        /// 服务节点ID
        /// </summary>
        [SugarColumn(ColumnDescription = "服务节点ID")]
        public long ServerId
        {
            get;
            set;
        }

        [SugarColumn(ColumnDescription = "事件源ID")]
        public int SysEventId
        {
            get;
            set;
        }
        /// <summary>
        /// 事件Key
        /// </summary>
        [SugarColumn(ColumnDescription = "事件Key", Length = 36, ColumnDataType = "varchar")]
        public string EventKey { get; set; }
        /// <summary>
        /// 模块名
        /// </summary>
        [SugarColumn(ColumnDescription = "事件模块名", Length = 50, ColumnDataType = "varchar")]
        public string Module { get; set; }
        /// <summary>
        /// 事件级别
        /// </summary>
        [SugarColumn(ColumnDescription = "事件级别")]
        public byte EventLevel
        {
            get;
            set;
        }
        /// <summary>
        /// 事件类型
        /// </summary>
        [SugarColumn(ColumnDescription = "事件类型")]
        public byte EventType
        {
            get;
            set;
        }
        /// <summary>
        /// 事件提示模板
        /// </summary>
        [SugarColumn(ColumnDescription = "事件提示模板", Length = 500)]
        public string MsgTemplate
        {
            get;
            set;
        }
        /// <summary>
        /// 事件配置项
        /// </summary>
        [SugarColumn(ColumnDescription = "事件配置项", Length = 1000, ColumnDataType = "varchar")]
        public string EventConfig
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        [SugarColumn(ColumnDescription = "是否启用")]
        public bool IsEnable
        {
            get;
            set;
        }
    }
}
