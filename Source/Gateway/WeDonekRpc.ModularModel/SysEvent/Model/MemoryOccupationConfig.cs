namespace WeDonekRpc.ModularModel.SysEvent.Model
{
    public class MemoryOccupationConfig
    {
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
        public int Interval { get; set; }
    }
}
