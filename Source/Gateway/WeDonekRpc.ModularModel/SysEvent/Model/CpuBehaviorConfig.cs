namespace WeDonekRpc.ModularModel.SysEvent.Model
{
    public class CpuBehaviorConfig
    {
        /// <summary>
        /// Cpu阈值
        /// </summary>
        public short Threshold
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
