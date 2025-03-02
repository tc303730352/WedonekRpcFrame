using WeDonekRpc.ModularModel.SysEvent.Model;

namespace WeDonekRpc.Modular.SysEvent.Model
{
    public class BasicEvent
    {
        public BasicEvent (ServiceSysEvent obj)
        {
            this.EventId = obj.EventId;
        }
        /// <summary>
        /// 系统事件ID
        /// </summary>
        public long EventId { get; }

    }
}
