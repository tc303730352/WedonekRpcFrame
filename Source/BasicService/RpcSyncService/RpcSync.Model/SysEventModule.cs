using WeDonekRpc.ModularModel;

namespace RpcSync.Model
{
    public class SysEventModule
    {
        /// <summary>
        /// 事件ID
        /// </summary>
        public int Id { get; set; }

        public string EventName { get; set; }
        /// <summary>
        /// 所属模块
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
        public string MsgTemplate
        {
            get;
            set;
        }
    }
}
