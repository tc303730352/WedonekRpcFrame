using WeDonekRpc.Helper;
using WeDonekRpc.ModularModel.SysEvent.Model;

namespace WeDonekRpc.Modular.SysEvent.Model
{
    internal class CpuResourceEvent : BasicEvent
    {
        public CpuResourceEvent (ServiceSysEvent obj) : base(obj)
        {
            CpuBehaviorConfig config = obj.EventConfig.Json<CpuBehaviorConfig>();
            this.Threshold = config.Threshold;
            this.SustainTime = config.SustainTime;
            this.Interval = config.Interval;
        }

        /// <summary>
        /// Cpu阈值
        /// </summary>
        public int Threshold
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
        public int Interval { get; set; }
    }
}
