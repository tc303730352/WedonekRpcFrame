using WeDonekRpc.Helper;
using WeDonekRpc.ModularModel.SysEvent.Model;

namespace WeDonekRpc.Modular.SysEvent.Model
{
    internal class MemoryOccupationEvent : BasicEvent
    {
        public MemoryOccupationEvent (ServiceSysEvent obj) : base(obj)
        {
            MemoryOccupationConfig config = obj.EventConfig.Json<MemoryOccupationConfig>();
            this.Threshold = config.Threshold * 1048576;
            this.SustainTime = config.SustainTime;
            this.Interval = config.Interval;
        }

        /// <summary>
        /// 内存阈值
        /// </summary>
        public long Threshold
        {
            get;
            set;
        }
        /// <summary>
        /// 持续时间
        /// </summary>
        public int SustainTime
        {
            get;
            set;
        }

        /// <summary>
        /// 上传间隔
        /// </summary>
        public int Interval
        {
            get;
            set;
        }

    }
}
