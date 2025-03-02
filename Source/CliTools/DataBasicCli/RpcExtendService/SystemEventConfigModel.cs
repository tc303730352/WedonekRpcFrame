using SqlSugar;

namespace DataBasicCli.RpcExtendService
{
    [SugarTable("SystemEventConfig", "服务节点事件配置表")]
    public class SystemEventConfigModel
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "事件ID")]
        public int Id
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "事件名", Length = 50)]
        public string EventName
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "模块名", Length = 50, ColumnDataType = "varchar")]
        public string Module
        {
            get;
            set;
        }
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

        [SugarColumn(ColumnDescription = "是否启用")]
        public bool IsEnable
        {
            get;
            set;
        }
    }
}
