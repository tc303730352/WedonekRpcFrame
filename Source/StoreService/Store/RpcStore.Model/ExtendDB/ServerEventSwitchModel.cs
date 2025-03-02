using RpcStore.RemoteModel;
using SqlSugar;

namespace RpcStore.Model.ExtendDB
{
    [SugarTable("ServerEventSwitch")]
    public class ServerEventSwitchModel
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
        public long RpcMerId { get; set; }
        /// <summary>
        /// 服务节点ID
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        public int SysEventId
        {
            get;
            set;
        }
        /// <summary>
        /// 事件Key
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 模块名
        /// </summary>
        public string Module { get; set; }
        /// <summary>
        /// 事件级别
        /// </summary>
        public SysEventLevel EventLevel
        {
            get;
            set;
        }
        /// <summary>
        /// 事件类型
        /// </summary>
        public SysEventType EventType
        {
            get;
            set;
        }
        /// <summary>
        /// 事件提示模板
        /// </summary>
        [SugarColumn(DefaultValue = "", IsNullable = false)]
        public string MsgTemplate
        {
            get;
            set;
        }
        /// <summary>
        /// 事件配置项
        /// </summary>
        [SugarColumn(DefaultValue = "", IsNullable = false)]
        public string EventConfig
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable
        {
            get;
            set;
        }
    }
}
