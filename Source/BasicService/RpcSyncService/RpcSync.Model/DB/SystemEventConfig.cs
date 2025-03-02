using SqlSugar;
using WeDonekRpc.ModularModel;

namespace RpcSync.Model.DB
{
    [SugarTable("SystemEventConfig")]
    public class SystemEventConfig
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 事件名
        /// </summary>
        public string EventName
        {
            get;
            set;
        }
        /// <summary>
        /// 模块
        /// </summary>
        public string Module
        {
            get;
            set;
        }
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
