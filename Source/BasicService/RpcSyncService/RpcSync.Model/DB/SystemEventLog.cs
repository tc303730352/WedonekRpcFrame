using WeDonekRpc.ModularModel;
using SqlSugar;

namespace RpcSync.Model.DB
{
    [SugarTable("SystemEventLog")]
    public class SystemEventLog
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 集群ID
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 节点ID
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 节点类型ID
        /// </summary>
        public long SystemTypeId
        {
            get;
            set;
        }
        /// <summary>
        /// 所在区域ID
        /// </summary>
        public int RegionId
        {
            get;
            set;
        }
        /// <summary>
        /// 事件源
        /// </summary>
        public int EventSourceId
        {
            get;
            set;
        }
        /// <summary>
        /// 事件名
        /// </summary>
        public string EventName { get; set; }
        /// <summary>
        /// 事件级别
        /// </summary>
        public SysEventLevel EventLevel
        {
            get;
            set;
        }

        /// <summary>
        /// 系统事件类别
        /// </summary>
        public SysEventType EventType
        {
            get;
            set;
        }
        /// <summary>
        /// 事件的说明
        /// </summary>
        [SugarColumn(DefaultValue = "", IsNullable = false)]
        public string EventShow
        {
            get;
            set;
        }
        /// <summary>
        /// 事件属性
        /// </summary>
        [SugarColumn(DefaultValue = "", IsNullable = false)]
        public string EventAttr
        {
            get;
            set;
        }
        /// <summary>
        /// 事件时间
        /// </summary>
        public DateTime AddTime
        {
            get;
            set;
        }
    }
}
