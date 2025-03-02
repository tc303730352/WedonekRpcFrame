using WeDonekRpc.Helper;
using WeDonekRpc.ModularModel;
using WeDonekRpc.ModularModel.SysEvent.Model;

namespace WeDonekRpc.Modular.SysEvent.Model
{
    internal class BehaviorEvent : BasicEvent
    {
        public BehaviorEvent(ServiceSysEvent obj) : base(obj)
        {
            BehaviorConfig config = obj.EventConfig.Json<BehaviorConfig>();
            this.Threshold = config.Threshold;
            this.RecordRange = config.RecordRange;
        }

        /// <summary>
        /// 发送超时阈值
        /// </summary>
        public uint Threshold
        {
            get;
            set;
        }

        /// <summary>
        /// 日志记录范围
        /// </summary>
        public LogRecordRange RecordRange
        {
            get;
            set;
        }

    }
}
