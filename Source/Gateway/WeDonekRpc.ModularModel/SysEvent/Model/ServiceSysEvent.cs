namespace WeDonekRpc.ModularModel.SysEvent.Model
{
    public class ServiceSysEvent
    {
        /// <summary>
        /// 系统事件ID
        /// </summary>
        public long EventId
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
        /// 事件配置项
        /// </summary>
        public string EventConfig
        {
            get;
            set;
        }
    }
}
